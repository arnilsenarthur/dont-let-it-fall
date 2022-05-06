using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class HUDScript : MonoBehaviour
    {
        [SerializeField]
        private VariableTime time;

        private float timeValue => time.Value;

        [SerializeField] 
        private Image timeClock;
        
        void Update()
        {
            if (timeValue >= .5f)
            {
                if(timeClock.fillClockwise)
                    timeClock.fillClockwise = false;
                
                timeClock.fillAmount = 1 - ((timeValue - .5f) / .5f);
            }
            else
            {
                if(!timeClock.fillClockwise)
                    timeClock.fillClockwise = true;
                
                timeClock.fillAmount = (timeValue) / .5f;
            }
        }
    }
}
