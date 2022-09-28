using System.Collections;
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
        Grab,
        Drop
    }

    public class EntityCrewmate : Selectable
    { 
        public const float PATH_CHECK_INTERVAL = 1F;

        public Transform marker;
        public Action action = Action.Idle;
        public Rigidbody bodyHips;
        public Vector3 defaultBodyHipsRotation = new Vector3(0f, 0f, 0f);

        public NavMeshAgent agent;
        public Transform ship;
        public Transform targetHolder;

        public GameObject targetGrabbableObject;
        public GameObject grabbedObject;
    
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
        public EntityGrabLimb[] grabLimbs;

        [Header("STATE")]
        public bool isGrounded = false;
        public Vector3 groundNormal = Vector3.up;

        #region Private Fields
        private Quaternion[] _startLimbRotations;
        private bool _assembled = true;
        private Rigidbody _rigidbody;
        private Vector3 _bodyHipsOffset;
        private float _lastWalkStart = 0; 
        private float _lastPathCheck = 0;
        #endregion

        #region Unity Callbacks
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _startLimbRotations = new Quaternion[limbs.Length];
            for (int i = 0; i < limbs.Length; i++)
                _startLimbRotations[i] = limbs[i].joint.transform.localRotation;

            _rigidbody.centerOfMass = Vector3.down;

            _bodyHipsOffset = bodyHips.transform.localPosition;

            SetAssembled(true);

            agent.updatePosition = true;
            agent.updateRotation = true;
        }

        private void OnEnable()
        {
            if(targetHolder == null)
            {
                targetHolder = new GameObject().transform;
                targetHolder.parent = transform.parent;
                targetHolder.name = $"{name}:Target";
            } 

            targetHolder.parent = ship;
        }

        private void OnDisable() 
        {
            if(targetHolder != null)
                Destroy(targetHolder.gameObject);
        }

        private void Update()
        {
            marker.LookAt(Camera.main.transform);

            bool walking = (action != Action.Idle) && agent.remainingDistance > 0.5f;

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
                case Action.Drop:
                    {
                        Vector3 tpos = agent.transform.position;
                        Vector3 deltaPos = (tpos - transform.position);
                        Vector3 fw = new Vector3(deltaPos.x, 0, deltaPos.z).normalized;
                        float angle = Vector3.SignedAngle(Vector3.forward, fw, Vector3.up);
                        float nAngle = Mathf.LerpAngle(transform.eulerAngles.y, angle, Time.fixedDeltaTime * 3f);
                        transform.eulerAngles = new Vector3(0, nAngle,0);

                        float walkSpeed = GetWalkSpeed();
                        
                        agent.speed = deltaPos.magnitude > 1f ? 0 : walkSpeed;

                        velocity.x = deltaPos.normalized.x * walkSpeed;
                        velocity.z = deltaPos.normalized.z * walkSpeed;

                        if(agent.remainingDistance < 0.25f && Time.time > _lastWalkStart + 0.5f)
                        {
                            if(action == Action.Drop)
                                ReleaseObject();

                            action = Action.Idle;
                            agent.Warp(transform.position);
                        }
                        else if(_lastPathCheck + PATH_CHECK_INTERVAL < Time.time)
                        {
                            _lastPathCheck = Time.time;
                            agent.SetDestination(targetHolder.position);
                        }
                    }
                    break;

                case Action.Grab:
                    {
                        Vector3 tpos = agent.transform.position;
                        Vector3 deltaPos = (tpos - transform.position);
                        Vector3 fw = new Vector3(deltaPos.x, 0, deltaPos.z).normalized;
                        float angle = Vector3.SignedAngle(Vector3.forward, fw, Vector3.up);
                        float nAngle = Mathf.LerpAngle(transform.eulerAngles.y, angle, Time.fixedDeltaTime * 3f);
                        transform.eulerAngles = new Vector3(0, nAngle,0);

                        float walkSpeed = GetWalkSpeed();
                        
                        agent.speed = deltaPos.magnitude > 1f ? 0 : walkSpeed;

                        velocity.x = deltaPos.normalized.x * walkSpeed;
                        velocity.z = deltaPos.normalized.z * walkSpeed;

                        if(Vector3.Distance(transform.position, targetGrabbableObject.transform.position) < 1f && Time.time > _lastWalkStart + 0.5f)
                        {
                            GrabObject(targetGrabbableObject);
                            targetGrabbableObject = null;
                            action = Action.Idle;
                            agent.Warp(transform.position);
                        }
                        else if(_lastPathCheck + PATH_CHECK_INTERVAL < Time.time)
                        {
                            targetHolder.position = targetGrabbableObject.transform.position;
                            _lastPathCheck = Time.time;
                            agent.SetDestination(targetHolder.position);
                        }
                    }
                    break;
            }      

             _rigidbody.velocity = velocity;     
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
            //if(action == Action.Idle)
            {   
                if(gameObject.tag == "Ship")
                {
                    agent.Warp(transform.position);
                    targetHolder.position = position;
                    agent.SetDestination(targetHolder.position);
                    action = grabbedObject == null ? Action.Walking : Action.Drop;
                    _lastWalkStart = _lastPathCheck = Time.time;
                    return true;
                }   

                if(gameObject.tag == "Grabbable" && grabbedObject == null)
                {
                    agent.Warp(transform.position);
                    targetHolder.position = gameObject.transform.position;
                    agent.SetDestination(targetHolder.position);
                    action = Action.Grab;
                    _lastWalkStart = _lastPathCheck = Time.time;
                    this.targetGrabbableObject = gameObject;
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

        #region Grabbing Things
        public void GrabObject(GameObject grabbableObject)
        {
            if(grabbedObject != null)
                return;

            grabbedObject = grabbableObject;
            Rigidbody rigidbody = grabbedObject.GetComponent<Rigidbody>();
            Debug.Log("Grabbed " + grabbedObject.name);
            rigidbody.mass *= 0.1f;

            foreach (EntityGrabLimb limb in grabLimbs)
            {
                ConfigurableJoint joint = grabbedObject.AddComponent<ConfigurableJoint>();
                joint.anchor = new Vector3(0, 0, 0);
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;
                joint.targetPosition = new Vector3(0, 0, 0);
                joint.connectedBody = limb.hand;
                //joint.projectionDistance = 0.5f;
                //joint.linearLimit = new SoftJointLimit() { limit = 0.5f };
            } 
        }

        public void ReleaseObject()
        {
            if (grabbedObject != null)
            {
                foreach (Joint joint in grabbedObject.GetComponents<Joint>())
                    Destroy(joint);

                Rigidbody rigidbody = grabbedObject.GetComponent<Rigidbody>();
                Debug.Log("Releasing: " + rigidbody.mass);
                rigidbody.mass *= 10f;

                //grabbedWeight.value = 0;
                grabbedObject = null;

                //for (int i = 0; i < grabLimbs.Length; i++)
                //grabLimbsGrabbing[i] = false;
            }
        }
        #endregion
    }
}