using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Outpost;
using UnityEngine;

namespace DontLetItFall.Outpost
{
    public class MerchantCamera : MonoBehaviour
    {
        public float _lerpSpeed = 0.1f;

        public Transform _merchant;
        private Transform _mainCamera;
        private bool toMerchant = false;
        
        private Camera _thisCamera => GetComponent<Camera>();

        private void Awake()
        {
            OutpostManager._mainCamera = Camera.main;
            OutpostManager._merchantCameraScript = this;
            gameObject.SetActive(false);
        }

        public void EnableCam()
        {
            _merchant = OutpostManager._currentMerchant.cameraTarget;
            _mainCamera = OutpostManager._mainCamera.transform;
            
            transform.position = _mainCamera.position;
            transform.rotation = _mainCamera.rotation;
            _thisCamera.fieldOfView = OutpostManager._mainCamera.orthographicSize * 4;
            
            toMerchant = true;
        }
        
        public void DisbleCam()
        {
            _mainCamera = OutpostManager._mainCamera.transform;
            
            toMerchant = false;
        }

        private void FixedUpdate()
        {
            if (toMerchant)
            {
                CameraGo();
            }
            else
            {
                CameraBack();
            }
        }

        private void CameraGo()
        {
            if (Vector3.Distance(transform.position, _merchant.position) > .1f)
            {
                _thisCamera.fieldOfView = Mathf.Lerp(_thisCamera.fieldOfView, 60, _lerpSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, _merchant.position, _lerpSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, _merchant.rotation, _lerpSpeed * Time.deltaTime);
            }else if(Input.GetKey(KeyCode.E)) DisbleCam();
        }

        private void CameraBack()
        {
            float FOV = (OutpostManager._mainCamera.orthographicSize * 4);
            
            _thisCamera.fieldOfView = Mathf.Lerp(_thisCamera.fieldOfView, FOV, _lerpSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, _mainCamera.position, _lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _mainCamera.rotation, _lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _mainCamera.position) < 1f)
            {
                gameObject.SetActive(false);
            }
        }
        
        //☻
    }
}
