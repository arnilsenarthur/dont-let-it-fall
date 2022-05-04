using UnityEngine;
using UnityEngine.Events;
using DontLetItFall.Data;
using DontLetItFall.Utils;

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
        public GameObject grabbedObject = null;

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
        public LayerMask groundLayerMask;

        [Header("LIMBS")]
        public Limb[] limbs;
        public GameObject limbsParent;

        [Header("GRAB")]
        public GrabLimb[] grabLimbs;

        [Header("STATE")]
        public bool isGrounded = false;
        #endregion

        #region Private Fields
        private Quaternion[] _startLimbRotations;
        private bool _assembled = true;
        private Rigidbody _rigidbody;
        #endregion

        #region Unity Methods
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _startLimbRotations = new Quaternion[limbs.Length];
            for (int i = 0; i < limbs.Length; i++)
                _startLimbRotations[i] = limbs[i].joint.transform.localRotation;

            _rigidbody.centerOfMass = Vector3.down;

            SetAssembled(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
                SetAssembled(!_assembled);

            #region Animation
            for (int i = 0; i < limbs.Length; i++)
            {
                Limb limb = limbs[i];
                Quaternion target = limb.target.localRotation;
                ConfigurableJointExtensions.SetTargetRotationLocal(limb.joint, target, _startLimbRotations[i]);
            }
            #endregion
        }

        private void FixedUpdate()
        {
            #region Update Grounded
            isGrounded = UnityEngine.Physics.CheckCapsule(
                transform.position + Vector3.down * 0.995f,
                transform.position + Vector3.down * 1.005f,
                0.25f,
                groundLayerMask
            );
            #endregion

            //Stand Assembled
            if (_assembled)
            {
                bodyHips.transform.localRotation = Quaternion.Lerp(bodyHips.transform.localRotation, Quaternion.identity, Time.deltaTime * wakeUpSpeed);
                bodyHips.transform.localPosition = Vector3.Lerp(bodyHips.transform.localPosition, Vector3.zero, Time.deltaTime * wakeUpSpeed);
            }
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

                    FixedJoint joint = grabbedObject.AddComponent<FixedJoint>();
                    joint.connectedBody = limb.hand;
                    joint.breakForce = 500;

                    PlayerInteraction interaction = new PlayerInteraction();
                    interaction.targetObject = grabbedObject;
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
                {
                    Destroy(joint);
                }

                grabbedObject = null;
            }
        }

        public void Jump()
        {
            _rigidbody.velocity += (_rigidbody.velocity.y * Vector3.down) + Vector3.up * 8f;
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