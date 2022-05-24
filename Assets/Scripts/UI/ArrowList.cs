using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DontLetItFall
{
    public class ArrowList : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        
        [SerializeField] 
        private int _value;
        
        public int GetValue => _value;
        
        [SerializeField]
        private List<string> _options;
        
        [SerializeField]
        private UnityEvent OnValueChanged;

        private void UpdateText()
        {
            _text.text = _options[_value];
        }

        public void SetValue(int value, bool eventTrigger = false)
        {
            _value = value;
            UpdateText();
            
            if (eventTrigger)
                OnValueChanged.Invoke();
        }

        public void Next()
        {
            if (_value < _options.Count - 1)
            {
                _value++;
            }else
            {
                _value = 0;
            }
            
            UpdateText();
            OnValueChanged.Invoke();
        }

        public void Previous()
        {
            if (_value > 0)
            {
                _value--;
            }
            else
            {
                _value = _options.Count - 1;
            }

            UpdateText();
            OnValueChanged.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateText();
        }
#endif
    }
}
