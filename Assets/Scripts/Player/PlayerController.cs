using UnityEngine;
using DontLetItFall.InputSystem;
using DontLetItFall.Utils;
using DontLetItFall.Entity.Player;

namespace DontLetItFall.Player
{
    [System.Serializable]
    public class PlayerControllerInput : InputInterface
    {
        [Binding("MoveX", BindingMode.Raw)]
        public float moveX;

        [Binding("MoveY", BindingMode.Raw)]
        public float moveY;

        [Binding("Jump", BindingMode.Down)]
        public bool jump;

        [Binding("Grab", BindingMode.Default)]
        public bool grabbing;
    }

    public class PlayerController : MonoBehaviour
    {
        #region Public Fields
        [Header("INPUT")]
        public PlayerControllerInput input;
        [Header("SETTINGS")]
        public float walkSpeed = 3f;
        public float balanceForce = 3f;
        public float rotationSpeed = 5f;
        public LayerMask groundLayerMask;
        [Header("BODY")]
        public Transform bodyParts;
        [Header("LIMBS")]
        public ConfigurableJoint[] limbs;
        public Transform[] targetLimbs;
        [Header("STATE")]
        public bool isGrounded = false;
        public bool keepBalance
        {
            get => _keepBalance;
            set
            {
                _keepBalance = value;
                UpdateKeepBalance();
            }
        }

        [HideInInspector]
        public Vector3 lastInput;
        #endregion

        #region Private Fields
        private Quaternion[] _startLimbRotations;
        private Rigidbody _rigidbody;
        private bool _keepBalance = true;
        private EntityPlayer _player = null;
        #endregion

        #region Private Methods
        private void Start()
        {
            _player = GetComponent<EntityPlayer>();
            _rigidbody = GetComponent<Rigidbody>();
            _startLimbRotations = new Quaternion[limbs.Length];
            for (int i = 0; i < limbs.Length; i++)
                _startLimbRotations[i] = limbs[i].transform.localRotation;

            UpdateKeepBalance();
        }

        private void Update()
        {
            for (int i = 0; i < limbs.Length; i++)
            {
                Quaternion target = targetLimbs[i].localRotation;
                ConfigurableJointExtensions.SetTargetRotationLocal(limbs[i], target, _startLimbRotations[i]);
            }

            if (input.grabbing && _player.grabbedObject == null)
            {
                _player.GrabObject();
            }
            else if (!input.grabbing && _player.grabbedObject != null)
            {
                _player.ReleaseObject();
            }
        }

        private void FixedUpdate()
        {
            this.input.Update();

            #region Update Grounded
            isGrounded = UnityEngine.Physics.CheckCapsule(
                transform.position + Vector3.down * 0.995f,
                transform.position + Vector3.down * 1.005f,
                0.25f,
                groundLayerMask
            );
            #endregion

            if (_player.Assembled)
            {
                #region Input
                Vector3 input = new Vector3(this.input.moveX, 0f, this.input.moveY).normalized;
                lastInput = input;
                #endregion

                #region Walk
                float y = _rigidbody.velocity.y;
                Vector3 velocity = input * walkSpeed;
                velocity.y = y;
                _rigidbody.velocity = velocity;
                #endregion

                #region Jump
                if (this.input.jump && isGrounded)
                {
                    _rigidbody.velocity += Vector3.up * 8f;
                }
                #endregion

                #region Rotation
                //Rotate towards input
                if (input.magnitude > 0f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(input);
                    bodyParts.rotation = Quaternion.Slerp(bodyParts.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
                    bodyParts.localEulerAngles = new Vector3(0f, bodyParts.localEulerAngles.y, 0f);
                }
                #endregion

                #region Self-Balance
                if (_keepBalance)
                {
                    Quaternion target = Quaternion.identity;
                    _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * balanceForce));
                }
                #endregion
            }
        }

        private void UpdateKeepBalance()
        {
            if (_keepBalance)
                _rigidbody.centerOfMass = Vector3.down;
            else
                _rigidbody.centerOfMass = Vector3.zero;
        }
        #endregion
    }
}