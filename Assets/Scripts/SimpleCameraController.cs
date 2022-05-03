using UnityEngine;

namespace DontLetItFall.Core
{
    public class SimpleCameraController : MonoBehaviour
    {
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
    }
}