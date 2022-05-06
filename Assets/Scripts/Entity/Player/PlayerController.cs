using UnityEngine;
using DontLetItFall.InputSystem;
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
        public Vector3 currentMoveInput;
        #endregion

        #region Private Fields
        private EntityPlayer _player = null;
        private Rigidbody _rigidbody;
        #endregion

        #region Public Fields
        
        #endregion

        #region Private Methods
        private void Start()
        {
            _player = GetComponent<EntityPlayer>();
            _rigidbody = _player.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            this.input.Update();

            #region Grab Input
            bool grabbing = input.grabbing && _player.Assembled;

            if (grabbing && _player.grabbedObject == null)
            {
                _player.GrabObject();
            }
            else if (!grabbing && _player.grabbedObject != null)
            {
                _player.ReleaseObject();
            }
            #endregion 

            #region Jump Input
            
            if (this.input.jump && _player.isGrounded && _player.Assembled)
            {
                _player.Jump();
            }
            #endregion
        }

        private void FixedUpdate()
        {
            if (_player.Assembled)
            {
                #region Input
                Vector3 input = new Vector3(this.input.moveX, 0f, this.input.moveY).normalized;
                this.currentMoveInput = input;
                #endregion

                #region Walk
                Vector3 velocity = new Vector3(0,_rigidbody.velocity.y,0);

                Vector3 toadd = Vector3.ProjectOnPlane(input,_player.isGrounded ? _player.groundNormal : Vector3.up) * _player.GetWalkSpeed();
                toadd.y *= Time.fixedDeltaTime;
                velocity += toadd;

                _rigidbody.velocity = velocity;
                #endregion

                #region Rotation
                //Rotate towards input
                if (input.magnitude > 0f)
                {
                    float rotSpeed =  _player.grabbedObject == null ? _player.rotationSpeed : _player.rotationSpeedWhileCarrying;
                    Transform transform = _player.limbsParent.transform;
                    Quaternion targetRotation = Quaternion.LookRotation(input);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotSpeed);
                    transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
                }
                #endregion

                #region Self-Balance
                Quaternion target = Quaternion.identity;
                _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * _player.balanceForce));
                #endregion

                if(_player.grabbedObject != null)
                {
                    //add velocity
                    _player.grabbedObject.GetComponent<Rigidbody>().velocity = _rigidbody.velocity;
                }
            }
        }

        #endregion
    }
}