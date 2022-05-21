using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class SettingsScript : MonoBehaviour
    {
        [SerializeField] 
        private ScrollRect _scrollRect;

        [SerializeField] 
        private RectTransform _selectImage;
        
        [SerializeField]
        private Button[] _settingsButtons;
        
        [SerializeField] 
        private int _value;
        
        [SerializeField]
        private RectTransform[] _settingsContents;

        private void Start()
        {
        }

        public void ChangeValue(int value)
        {
            _value = value;
            UpdateContent();
        }

        private void UpdateContent()
        {
            foreach (var content in _settingsContents)
            {
                content.gameObject.SetActive(false);
            }

            foreach (var btt in _settingsButtons)
            {
                btt.interactable = true;
            }
            
            _settingsButtons[_value].interactable = false;
            _settingsContents[_value].gameObject.SetActive(true);
            _scrollRect.content = _settingsContents[_value];
            
            _selectImage.position = _settingsButtons[_value].GetComponent<RectTransform>().position;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateContent();
        }
#endif
    }
}
