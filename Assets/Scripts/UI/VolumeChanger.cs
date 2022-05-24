using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Data;
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
        
        [SerializeField]
        private SaveManager saveManager;
        
        [SerializeField]
        private int _volumeIndex;

        public void OnValueChanged()
        {
            _volumeText.text = ((int)(_volumeSlider.value * 100)).ToString() + "%";

            switch (_volumeIndex)
            {
                case 0:
                    saveManager.saveData.masterVolume = _volumeSlider.value;
                    break;
                case 1:
                    saveManager.saveData.musicVolume = _volumeSlider.value;
                    break;
                case 2:
                    saveManager.saveData.sfxVolume = _volumeSlider.value;
                    break;
            }
            
            saveManager.ApplyVariables();
            saveManager.SaveData();
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