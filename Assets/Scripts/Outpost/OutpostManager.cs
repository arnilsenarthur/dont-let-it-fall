using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DontLetItFall.Outpost
{
    public static class OutpostManager
    {
        public static Camera _mainCamera;
        public static MerchantCamera _merchantCameraScript;
        public static Camera _merchantCamera => _merchantCameraScript.GetComponent<Camera>();
        
        public static OutpostMerchant[] _merchants;
        public static OutpostMerchant _currentMerchant;
        
        public static OutpostPlayerMovement _player;

        public static void InteractMerchant(OutpostMerchant merchant)
        {
            _currentMerchant = merchant;
            
            _merchantCamera.depth = 0;
            _merchantCameraScript.EnableCam();
            _merchantCamera.gameObject.SetActive(true);
        }
        
        public static void ExitMerchant()
        {
            _merchantCameraScript.DisbleCam();
        }
    }    
}
