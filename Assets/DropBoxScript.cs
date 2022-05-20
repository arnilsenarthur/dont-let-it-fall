using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontLetItFall
{
    public class DropBoxScript : MonoBehaviour
    {        
        [SerializeField]
        private GameObject parachute;

        [SerializeField] 
        private float distanceToEnd;
        
        private Rigidbody rb;
        
        private Animator animator;
        
        private bool isFalling = true;

        private void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            rb.mass /= 4;
        }

        private void Update()
        {
            if (transform.childCount == 0) return;
            
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            
            if(!isFalling) return;
            
            if (UnityEngine.Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1000))
            {
                if (hit.distance <= distanceToEnd)
                {
                    animator.Play("Ground");
                    Debug.Log("Grounded");
                    isFalling = false;
                    rb.mass *= 4;
                    StartCoroutine(DisableAnimator());
                }
                //Debug.Log(hit.distance);
            }
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
        }

        private IEnumerator DisableAnimator()
        {
            yield return new WaitForSeconds(2);
            animator.enabled = false;
        }
    }
}
