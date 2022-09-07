using UnityEngine;
using DontLetItFall.Utils;
using DontLetItFall.Variables;
using UnityEngine.AI;

namespace New.Entity
{
    [System.Serializable]
    public class Limb
    {
        public ConfigurableJoint joint;

        public Transform stillTarget;
        public Transform walkingTarget;

        public float assembledPositionSpring = 60f;
        public float disassembledPositionSpring = 5f;

        public Transform GetTarget(bool walking)
        {
            return walking ? walkingTarget : stillTarget;
        }
    }

    public enum Action
    {
        Idle,
        Walking,
    }

    public class EntityCrewmate : Selectable
    { 
        public Transform marker;
        public Action action = Action.Idle;
        public Rigidbody bodyHips;
        public Vector3 defaultBodyHipsRotation = new Vector3(0f, 0f, 0f);
    
        #region Public Properties
        public bool Assembled
        {
            get { return _assembled; }
            set { SetAssembled(value); }
        }
        #endregion

        [Header("SETTINGS")]
        public Value<float> walkSpeed = new Value<float>(4);
        public float wakeUpSpeed = 1f;
        public float balanceForce = 3f;
        public Value<float> rotationSpeed = new Value<float>(4);
        public float rotationSpeedWhileCarrying = 1f;
        public LayerMask groundLayerMask;
        public float groundCheckDistance = 0.8f;
        public Value<float> jumpForce = new Value<float>(8);
        public float gravityForce = 20f;

        [Header("LIMBS")]
        public Limb[] limbs;
        public GameObject limbsParent;
        public int headIndex = 0;

        [Header("STATE")]
        public bool isGrounded = false;
        public Vector3 groundNormal = Vector3.up;

        #region Private Fields
        private Quaternion[] _startLimbRotations;
        private bool _assembled = true;
        private Rigidbody _rigidbody;
        private Vector3 _bodyHipsOffset;
        private NavMeshAgent _agent;
        #endregion

        #region Unity Callbacks
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody>();
            _startLimbRotations = new Quaternion[limbs.Length];
            for (int i = 0; i < limbs.Length; i++)
                _startLimbRotations[i] = limbs[i].joint.transform.localRotation;

            _rigidbody.centerOfMass = Vector3.down;

            _bodyHipsOffset = bodyHips.transform.localPosition;

            SetAssembled(true);

            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _agent.speed = walkSpeed.value;
        }

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.J))
                SetAssembled(!_assembled);*/

            marker.LookAt(Camera.main.transform);

            bool walking = action == Action.Walking && _agent.remainingDistance > 0.5f;

            #region Animation
            if (_assembled)
                for (int i = 0; i < limbs.Length; i++)
                {
                    Limb limb = limbs[i];
                    Quaternion target = limb.GetTarget(walking).localRotation;
                    ConfigurableJointExtensions.SetTargetRotationLocal(limb.joint, target, _startLimbRotations[i]);
                }
            #endregion

            #region Head Look At
            /*
            Vector3 playerForward = limbsParent.transform.forward;
            Vector3 viewTarget = Camera.main.transform.position;
            Limb head = limbs[headIndex];
            Vector3 dir = viewTarget - head.joint.transform.position;

            if (Vector3.Dot(playerForward, dir) > 0)
            {
                Quaternion r = Quaternion.LookRotation(dir);
                r = r * Quaternion.Inverse(head.joint.transform.parent.rotation);
                head.joint.SetTargetRotationLocal(r, _startLimbRotations[headIndex]);
            }
            else
                head.joint.targetRotation = Quaternion.identity;
            */
            #endregion
        }

        private void FixedUpdate()
        {
            #region Update Grounded
            Debug.DrawLine(transform.position - new Vector3(0, groundCheckDistance, 0), transform.position - new Vector3(0, groundCheckDistance + 0.25f, 0), Color.red);
            isGrounded = UnityEngine.Physics.CheckSphere(transform.position - new Vector3(0, groundCheckDistance, 0), 0.25f, groundLayerMask);
            #endregion

            Vector3 velocity = _rigidbody.velocity;

            if (isGrounded)
            {
                #region Ground Normal 
                if (UnityEngine.Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f, groundLayerMask))
                {
                    groundNormal = hit.normal;
                }
                #endregion

                //velocity.y = Mathf.Max(velocity.y, 0f);
                //velocity -= groundNormal * 9.81f * Time.fixedDeltaTime;
            }
            else
            {
                velocity.y -= gravityForce * Time.fixedDeltaTime;
            }

            if (_assembled)
            {
                //Stand Up
                bodyHips.transform.localRotation = Quaternion.Lerp(bodyHips.transform.localRotation, Quaternion.Euler(defaultBodyHipsRotation), Time.deltaTime * wakeUpSpeed);
                bodyHips.transform.localPosition = Vector3.Lerp(bodyHips.transform.localPosition, _bodyHipsOffset, Time.deltaTime * wakeUpSpeed);
            }
            else
            {
                //Drag
                velocity.x *= 1f - Time.fixedDeltaTime * 2;
                velocity.z *= 1f - Time.fixedDeltaTime * 2;
            }

            float y = velocity.y;

            switch(action)
            {
                case Action.Idle:
                    _rigidbody.velocity = new Vector3(0, y, 0);
                    break;

                case Action.Walking:
                    float angle = Vector3.SignedAngle(Vector3.forward, _agent.desiredVelocity.normalized, Vector3.up);
                    float nAngle = Mathf.LerpAngle(transform.eulerAngles.y, angle, Time.fixedDeltaTime * 3f);

                    transform.eulerAngles = new Vector3(0, nAngle,0);
                    //_rigidbody.MoveRotation(transform.rotation * Quaternion.Euler(0, nAngle - transform.eulerAngles.y, 0));
                    velocity.x = _agent.desiredVelocity.normalized.x * GetWalkSpeed();
                    velocity.z = _agent.desiredVelocity.normalized.z * GetWalkSpeed();

                    _rigidbody.velocity = velocity;
                    _agent.velocity = _rigidbody.velocity;

                    if(_agent.remainingDistance < 1f)
                    {
                        action = Action.Idle;
                        _agent.Warp(transform.position);
                    }
                    break;
            }           
        }
        #endregion

        public float GetWalkSpeed()
        {
            return walkSpeed.value;
        }

        public void SetAssembled(bool assembled)
        {
            bodyHips.isKinematic = assembled;
            _assembled = assembled;

            foreach (Limb limb in limbs)
            {
                ConfigurableJoint joint = limb.joint;
                JointDrive drive = joint.angularXDrive;
                drive.positionSpring = assembled ? limb.assembledPositionSpring : limb.disassembledPositionSpring;
                joint.angularXDrive = drive;

                drive = joint.angularYZDrive;
                drive.positionSpring = assembled ? limb.assembledPositionSpring : limb.disassembledPositionSpring;
                joint.angularYZDrive = drive;
            }
        }

        public override bool OnClick(Vector3 position, Vector3 normal, GameObject gameObject)
        {
            if(action == Action.Idle)
            {   
                if(gameObject.tag == "Ship")
                {
                    action = Action.Walking;
                    _agent.Warp(transform.position);
                    _agent.SetDestination(position);
                    return true;
                }      
            }

            return false;
        }

        public override void OnSelect()
        {
            marker.gameObject.SetActive(true);
        }

        public override void OnDeselect()
        {
            marker.gameObject.SetActive(false);
        }
    }
}