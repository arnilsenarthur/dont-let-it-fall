using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace DontLetItFall.Outpost
{
    public class OutpostMerchant : MonoBehaviour
    {
        public Transform cameraTarget;
        
        [SerializeField] private Image icon;

        private Camera mainCamera;
        private bool _isTrigger = false;
        private Camera _camera;

        private void Start()
        {
            icon.gameObject.SetActive(false);
            mainCamera = OutpostManager._mainCamera;
            _camera = OutpostManager._merchantCamera;
        }

        private void FixedUpdate()
        {
            icon.transform.LookAt(mainCamera.transform);

            if (_isTrigger)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if(!_camera.gameObject.activeSelf)
                        OutpostManager.InteractMerchant(this);
                }
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                icon.gameObject.SetActive(true);
                _isTrigger = true;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                icon.gameObject.SetActive(false);
                _isTrigger = false;
            }
        }
    }
}