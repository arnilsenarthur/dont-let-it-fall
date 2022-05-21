using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class SliderOptions : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private HorizontalLayoutGroup _layoutGroup;
        
        [SerializeField]
        private int[] _layoutSpacing = new int[9]{0, 165, 20, -25, -50, -65, -75, -80, -82};
        
        [Space(1f)]
        
        [SerializeField]
        private SliderOptionColor _sliderOptionColor;
        
        [Space(1f)]
        
        [SerializeField] 
        private int _value;

        [SerializeField] 
        private List<string> _options;

        private List<TextMeshProUGUI> textOptions = new List<TextMeshProUGUI>();
        
        [SerializeField]
        private UnityEvent OnValueChanged;

        private void Start()
        {
            UpdateSlider();
        }

        public void UpdateSlider()
        {
            _value = (int)_slider.value;
            
            int i = 0;
            foreach (var text in textOptions)
            {
                if(i == _value)
                    text.color = _sliderOptionColor._selectedColor;
                else
                    text.color = _sliderOptionColor._normalColor;
                
                i++;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            textOptions.Clear();

            int layoutChild = _layoutGroup.transform.childCount;
            for (int i = 0; i < layoutChild; i++)
            {
                var txt = _layoutGroup.transform.GetChild(i);
                if (i < _options.Count)
                {
                    txt.GetComponent<TextMeshProUGUI>().text = _options[i];
                    textOptions.Add(txt.GetComponent<TextMeshProUGUI>());
                    txt.gameObject.SetActive(true);
                }
                else
                {
                    txt.gameObject.SetActive(false);
                }
            }
            
            _layoutGroup.spacing = _layoutSpacing[_options.Count-1];
            SetSlider();
            UpdateSlider();
        }
        
#endif
        private void SetSlider()
        {
            _slider.wholeNumbers = true;
            _slider.minValue = 0;
            _slider.maxValue = _options.Count - 1;
            _slider.value = _value;
        }
    }

    [System.Serializable]
    public class SliderOptionColor
    {
        public Color _normalColor = Color.white;
        public Color _selectedColor = Color.green;
    }
}
