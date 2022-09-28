using UnityEngine;
using UnityEngine.Events;
using DontLetItFall.Data;
using DontLetItFall.Utils;
using DontLetItFall.Variables;

namespace DontLetItFall.Entity.Player
{
    public enum PlayerInteractionType
    {
        Grab,
        Interact
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
        public Vector3 defaultBodyHipsRotation = new Vector3(0f, 0f, 0f);

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
        public Value<float> walkSpeed = new Value<float>(4);
        public float wakeUpSpeed = 1f;
        public float balanceForce = 3f;
        public Value<float> rotationSpeed = new Value<float>(4);
        public float rotationSpeedWhileCarrying = 1f;
        public LayerMask groundLayerMask;
        public float groundCheckDistance = 0.8f;
        public Value<float> jumpForce = new Value<float>(8);
        public float gravityForce = 20f;
        public AnimationCurve walkSpeedWeightMultiplier;


        [Header("AUDIO")]
        public AudioSource walkingAudio;

        [Header("LIMBS")]
        public Limb[] limbs;
        public GameObject limbsParent;
        public int headIndex = 0;

        [Header("GRAB")]
        public GrabLimb[] grabLimbs;
        public bool[] grabLimbsGrabbing;
        public float grabWeightMultiplier = 0.1f;

        [Header("STATE")]
        public bool isGrounded = false;
        public Vector3 groundNormal = Vector3.up;
        public GameObject grabbedObject;
        public GameObject interactableObject;
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
            grabLimbsGrabbing = new bool[grabLimbs.Length];
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
            return walkSpeedWeightMultiplier.Evaluate(grabbedWeight.value) * walkSpeed.value;
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

            _rigidbody.velocity = velocity;
        }
        #endregion

        #region Actions
        public void GrabObject()
        {
            int i = 0;
            foreach (GrabLimb limb in grabLimbs)
            {
                if (limb.currentObject != null)
                {
                    if (grabbedObject != null && grabbedObject != limb.currentObject)
                        return;

                    Rigidbody rigidbody = limb.currentObject.GetComponent<Rigidbody>();
                    grabbedWeight.value = rigidbody.mass;
                    
                    Debug.Log("Grabbed " + limb.currentObject.name);

                    if (grabbedObject == null)  //Lighter to carry
                        rigidbody.mass *= grabWeightMultiplier;

                    grabbedObject = limb.currentObject;


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
                    joint.projectionDistance = 0.5f;
                    joint.linearLimit = new SoftJointLimit() { limit = 0.5f };

                    /*
                    FixedJoint joint = grabbedObject.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = limb.hand;
                    joint.breakForce = 500;
                    */

                    PlayerInteraction interaction = new PlayerInteraction();
                    interaction.targetObject = grabbedObject.gameObject;
                    interaction.type = PlayerInteractionType.Grab;

                    OnStopCanInteractWithObject.Invoke(interaction);

                    grabLimbsGrabbing[i] = true;
                }
                i++;
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
                rigidbody.mass /= grabWeightMultiplier;

                grabbedWeight.value = 0;
                grabbedObject = null;

                for (int i = 0; i < grabLimbs.Length; i++)
                    grabLimbsGrabbing[i] = false;
            }
        }

        public void Jump()
        {
            _rigidbody.velocity += (_rigidbody.velocity.y * Vector3.down) + Vector3.up * jumpForce.value;
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