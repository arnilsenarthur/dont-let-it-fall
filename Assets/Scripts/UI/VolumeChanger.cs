using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class VolumeChanger : MonoBehaviour
    {
        [SerializeField] 
        private Slider _volumeSlider;
        
        [SerializeField]
        private TextMeshProUGUI _volumeText;
        
        [SerializeField] [Range(0.0001f, 1)]
        private float _volume = 0.5f;

        public void OnValueChanged()
        {
            _volumeText.text = ((int)(_volumeSlider.value * 100)).ToString() + "%";
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            _volumeSlider.value = _volume;
            OnValueChanged();
        }
        #endif
    }
}