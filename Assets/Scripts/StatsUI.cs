using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private float maxValue = 100;

        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        [SerializeField] 
        private float currentValue = 100;
        
        public float CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        private float fillValue => currentValue / maxValue;
        
        [SerializeField]
        private Image fillImage;
        
        [SerializeField]
        private TextMeshProUGUI statsIcon;

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
                    statsIcon.color = Color.green;
                else
                    statsIcon.color = Color.white;
            }
        }
    }
}
