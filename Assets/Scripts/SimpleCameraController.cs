using UnityEngine;

namespace DontLetItFall.Core
{
    public class SimpleCameraController : MonoBehaviour
    {
        public Transform target;
        public float lookSpeed = 1f;
        public float moveSpeed = 5f;
        public AnimationCurve fovByDistance = new AnimationCurve(new Keyframe(0, 60), new Keyframe(1, 60));
        public AnimationCurve cameraXOffsetByPosition = new AnimationCurve(new Keyframe(-10, -7), new Keyframe(10, 7));
        public AnimationCurve heightOffsetByBoatAngle = new AnimationCurve(new Keyframe(-45, -5), new Keyframe(45, 5));

        private Camera _camera;
        private Vector3 _startPosition;

        public GameObject boat;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _startPosition = transform.position;
        }

        private void Update()
        {
            #region Rotation
            Vector3 relativePos = target.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, lookSpeed * Time.deltaTime);
            #endregion

            #region Zoom
            _camera.fieldOfView = fovByDistance.Evaluate(relativePos.magnitude);
            #endregion

            float targetY = target.position.y + 4f;

            float angle = boat.transform.eulerAngles.x;
   
            if (angle > 180)
                angle -= 360;

            targetY += heightOffsetByBoatAngle.Evaluate(angle);

            //float targetY = cameraYOffsetByHeight.Evaluate(target.position.y - _startPosition.y) + _startPosition.y;
            float targetX = cameraXOffsetByPosition.Evaluate(target.position.x - _startPosition.x) + _startPosition.x;
            //lerp to targetY
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetX, targetY, transform.position.z), moveSpeed * Time.deltaTime);
        }


        /*
        public Transform target;
        public float LookSpeed = 15f;

        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            Vector3 relativePos = target.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime);
        }
        */
    }
}