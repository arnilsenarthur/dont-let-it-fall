using UnityEngine;

namespace New.Entity
{   
    public class EntityPuppet : MonoBehaviour
    {
        public bool walking = false;
        public bool grabbing = false;

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

        public float walkAnimThreshold = 2f;
        private bool _wasWalking = false;
        
        private void Update()
        {
            float walkAnim = this.walking ? Mathf.Sin(Time.time * Mathf.Deg2Rad * walkAnimSpeed) : 0;
            LegL.localEulerAngles = walkAnimMax * walkAnim;
            LegR.localEulerAngles = walkAnimMax * -walkAnim;

            float f = (walkAnimMax * walkAnim).x;
            
            bool walking = f > walkAnimThreshold;

            _wasWalking = walking;

            //bool grabbingLeft = _player.grabLimbsGrabbing[1];
            //bool grabbingRight = _player.grabLimbsGrabbing[0];

            /*
            float factor = grabbingLeft ? 1f : 0f;
            ArmL.localEulerAngles = Vector3.Lerp(ArmLDefaultRotation, ArmLTargetRotation, factor);

            factor = grabbingRight ? 1f : 0f;
            ArmR.localEulerAngles = Vector3.Lerp(ArmRDefaultRotation, ArmRTargetRotation, factor);
            */

            float factor = grabbing ? 1f : 0f;
            ArmL.localEulerAngles = Vector3.Lerp(ArmLDefaultRotation, ArmLTargetRotation, factor);
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