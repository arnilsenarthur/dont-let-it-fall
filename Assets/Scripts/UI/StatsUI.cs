using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private Value<float> maxValue;

        public Value<float> MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        [SerializeField] 
        private Value<float> currentValue;
        
        public Value<float> CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        private float fillValue => currentValue.value / maxValue.value;
        
        [SerializeField]
        private Image fillImage;
        
        [SerializeField]
        private Image statsIcon;

        [SerializeField] [Range(0, 1)] 
        private float warningPercentage = .1f;

        private void Update()
        {
            fillImage.fillAmount = fillValue;

            if (fillValue <= warningPercentage)
            {
                if(fillValue <= 0)
                    statsIcon.color = Color.black;
                else
                    statsIcon.color = Color.red;
            }
            else
            {
                if(fillValue >= 1)
                    statsIcon.color = Color.white;
                else
                    statsIcon.color = Color.white;
            }
        }
    }
}
