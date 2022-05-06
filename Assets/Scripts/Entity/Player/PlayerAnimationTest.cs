using UnityEngine;
using DontLetItFall.Entity.Player;

namespace DontLetItFall.Player
{
    public class PlayerAnimationTest : MonoBehaviour
    {
        public PlayerController controller;
        private EntityPlayer _player;

        public Transform LegL;
        public Transform LegR;

        public Transform ArmL;
        public Transform ArmR;

        public Vector3 ArmLDefaultRotation;
        public Vector3 ArmLTargetRotation;

        public Vector3 ArmRDefaultRotation;
        public Vector3 ArmRTargetRotation;


        public float walkAnimSpeed = 60f;
        public Vector3 walkAnimMax = new Vector3(0.5f, 0.5f, 0.5f);
        
        private void Start() 
        {
            _player = controller.GetComponent<EntityPlayer>();
        }

        private void Update()
        {
            float walkAnim = Mathf.Sin(Time.time * Mathf.Deg2Rad * walkAnimSpeed);
            walkAnim = controller.currentMoveInput.magnitude * walkAnim;
            LegL.localEulerAngles = walkAnimMax * walkAnim;
            LegR.localEulerAngles = walkAnimMax * -walkAnim;

            bool grabbingLeft = _player.grabLimbsGrabbing[1];
            bool grabbingRight = _player.grabLimbsGrabbing[0];

            float factor = grabbingLeft ? 1f : 0f;
            ArmL.localEulerAngles = Vector3.Lerp(ArmLDefaultRotation, ArmLTargetRotation, factor);

            factor = grabbingRight ? 1f : 0f;
            ArmR.localEulerAngles = Vector3.Lerp(ArmRDefaultRotation, ArmRTargetRotation, factor);
        }

        /*
        public PlayerController controller;

        public Transform Arm;

        public Vector3 ArmDefaultRotation;
        public Vector3 ArmTargetRotation;

        public Transform LegA;
        public Transform LegB;

        public float walkAnimSpeed = 60f;

        public Vector3 walkAnimMax = new Vector3(0.5f, 0.5f, 0.5f);


        private void Update()
        {
            float factor = Mathf.Clamp01(Mathf.Sin(Time.time * Mathf.Deg2Rad * 45f) + 0.5f);
            Arm.localEulerAngles = Vector3.Lerp(ArmDefaultRotation, ArmTargetRotation, factor);


            float walkAnim = Mathf.Sin(Time.time * Mathf.Deg2Rad * walkAnimSpeed);

            walkAnim = controller.currentMoveInput.magnitude * walkAnim;

            LegA.localEulerAngles = walkAnimMax * walkAnim;
            LegB.localEulerAngles = walkAnimMax * -walkAnim;
        }
        */
    }
}