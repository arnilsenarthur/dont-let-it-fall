using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DontLetItFall.Outpost
{
    public class OupostShopItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject _shopItem;
        [SerializeField]
        private Image _shopItemImage;
        
        [Space]
        [SerializeField]
        [Range(.1f,10f)]
        private float floatingSpeed = 0.1f;
        [SerializeField]
        private float floatingWiggle = 1f;
        
        private float fixedY;
        private bool floatingUp = true;
        private Transform _camera;

        private float floatingVel => floatingSpeed * Time.fixedDeltaTime;
        private float minY => fixedY - floatingWiggle;
        private float maxY => fixedY + floatingWiggle;
        private Transform _itemTransform => _shopItem.transform;

        private void Start()
        {
            fixedY = _itemTransform.localPosition.y;
            _itemTransform.localPosition += new Vector3(0, Random.Range(-.2f, .21f), 0);
            floatingUp = Random.Range(0, 2) == 0;

            _shopItemImage.gameObject.SetActive(false);
            _camera = OutpostManager._mainCamera.transform;
        }

        private void FixedUpdate()
        {
            _shopItemImage.transform.LookAt(_camera);
            
            if (floatingUp)
            {
                _itemTransform.localPosition = Vector3.Lerp(_itemTransform.localPosition, new Vector3(_itemTransform.localPosition.x, fixedY + floatingWiggle, _itemTransform.localPosition.z), floatingVel);
                
                if(_itemTransform.localPosition.y >= maxY-.1f) floatingUp = false;
            }
            else
            {
                _itemTransform.localPosition = Vector3.Lerp(_itemTransform.localPosition, new Vector3(_itemTransform.localPosition.x, fixedY - floatingWiggle, _itemTransform.localPosition.z), floatingVel);
                
                if(_itemTransform.localPosition.y <= minY+.1f) floatingUp = true;
            }
        }
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _shopItemImage.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("Player"))
            {
                _shopItemImage.gameObject.SetActive(false);
            }
        }
    }    
}
