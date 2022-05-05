using UnityEngine;
using UnityEngine.Events;
using DontLetItFall.Data;
using DontLetItFall.Utils;
using DontLetItFall.Variables;

namespace DontLetItFall.Entity.Player
{
    public enum PlayerInteractionType
    {
        Grab
    }

    public struct PlayerInteraction
    {
        public PlayerInteractionType type;
        public GameObject targetObject;
    }

    [System.Serializable]
    public class Limb
    {
        public ConfigurableJoint joint;
        public Transform target;
        public float assembledPositionSpring = 60f;
        public float disassembledPositionSpring = 5f;
    }

    public class EntityPlayer : EntityBase
    {
        public PlayerStats stats;
        public Rigidbody bodyHips;
        
        #region Events
        public UnityEvent<PlayerInteraction> OnCanInteractWithObject;
        public UnityEvent<PlayerInteraction> OnStopCanInteractWithObject;
        #endregion

        #region Public Properties
        public bool Assembled
        {
            get { return _assembled; }
            set { SetAssembled(value); }
        }
        #endregion

        #region Public Fields
        [Header("SETTINGS")]
        public float walkSpeed = 3f;
        public float wakeUpSpeed = 1f;
        public float balanceForce = 3f;
        public float rotationSpeed = 5f;
        public float rotationSpeedWhileCarrying = 1f;
        public LayerMask groundLayerMask;
        public float groundCheckDistance = 0.8f;
        public float jumpForce = 15f;
        public float gravityForce = 20f;
        public AnimationCurve walkSpeedWeightMultiplier;

        [Header("LIMBS")]
        public Limb[] limbs;
        public GameObject limbsParent;
        public int headIndex = 0;

        [Header("GRAB")]
        public GrabLimb[] grabLimbs;

        [Header("STATE")]
        public bool isGrounded = false;
        public Vector3 groundNormal = Vector3.up;
        public GameObject grabbedObject;
        public Value<float> grabbedWeight;
        #endregion

        #region Private Fields
        private Quaternion[] _startLimbRotations;
        private bool _assembled = true;
        private Rigidbody _rigidbody;
        private Vector3 _bodyHipsOffset;
        #endregion

        #region Unity Methods
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _startLimbRotations = new Quaternion[limbs.Length];
            for (int i = 0; i < limbs.Length; i++)
                _startLimbRotations[i] = limbs[i].joint.transform.localRotation;

            _rigidbody.centerOfMass = Vector3.down;

            _bodyHipsOffset = bodyHips.transform.localPosition;

            SetAssembled(true);
        }

        public float GetWalkSpeed()
        {
            return walkSpeedWeightMultiplier.Evaluate(grabbedWeight.value) * walkSpeed;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
                SetAssembled(!_assembled);

            #region Animation
            if (_assembled)
                for (int i = 0; i < limbs.Length; i++)
                {
                    Limb limb = limbs[i];
                    Quaternion target = limb.target.localRotation;
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
                bodyHips.transform.localRotation = Quaternion.Lerp(bodyHips.transform.localRotation, Quaternion.identity, Time.deltaTime * wakeUpSpeed);
                bodyHips.transform.localPosition = Vector3.Lerp(bodyHips.transform.localPosition, _bodyHipsOffset, Time.deltaTime * wakeUpSpeed);
            }
            else
            {
                //Drag
                velocity.x *= 1f - Time.fixedDeltaTime * 2;
                velocity.z *= 1f - Time.fixedDeltaTime * 2;
            }

            _rigidbody.velocity = velocity;
        }
        #endregion

        #region Actions
        public void GrabObject()
        {
            foreach (GrabLimb limb in grabLimbs)
            {
                if (limb.currentObject != null)
                {
                    if (grabbedObject != null && grabbedObject != limb.currentObject)
                        return;

                    grabbedObject = limb.currentObject;
                    grabbedWeight.value = grabbedObject.GetComponent<Rigidbody>().mass;

                    /*
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
                    joint.projectionDistance = 0.01f;
                    joint.linearLimit = new SoftJointLimit(){limit = 0.01f};
                    */

                    FixedJoint joint = grabbedObject.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = limb.hand;
                    joint.breakForce = 500;

                    PlayerInteraction interaction = new PlayerInteraction();
                    interaction.targetObject = grabbedObject.gameObject;
                    interaction.type = PlayerInteractionType.Grab;

                    OnStopCanInteractWithObject.Invoke(interaction);
                }
            }
        }

        public void ReleaseObject()
        {
            if (grabbedObject != null)
            {
                foreach (Joint joint in grabbedObject.GetComponents<Joint>())
                    Destroy(joint);

                grabbedWeight.value = 0;
                grabbedObject = null;
            }
        }

        public void Jump()
        {
            _rigidbody.velocity += (_rigidbody.velocity.y * Vector3.down) + Vector3.up * jumpForce;
        }
        #endregion

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
    }
}