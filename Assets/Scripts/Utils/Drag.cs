using UnityEngine;

namespace DontLetItFall.Utils
{
    public class Drag : MonoBehaviour
    {
        public float drag = 0.5f;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 velocity = _rigidbody.velocity;

            float factor = 1f - drag * Time.fixedDeltaTime;
            velocity.x *= factor;
            velocity.z *= factor;

            _rigidbody.velocity = velocity;
        }
    }
}