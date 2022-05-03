using UnityEngine;

namespace DontLetItFall.Player
{
    public class PlayerAnimationTest : MonoBehaviour
    {
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

            walkAnim = controller.lastInput.magnitude * walkAnim;

            LegA.localEulerAngles = walkAnimMax * walkAnim;
            LegB.localEulerAngles = walkAnimMax * -walkAnim;
        }
    }
}