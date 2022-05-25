using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Data;
using DontLetItFall.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class HUDScript : MonoBehaviour
    {
        [Header("Clock")]
        [SerializeField]
        private VariableTime time;
        [SerializeField]
        private VariableString weekDay;
        [SerializeField]
        private Image timeClock;
        [SerializeField]
        private TextMeshProUGUI weekDayText;
        private float timeValue => time.Value;

        [Space]
        [Header("Gyroscope")]
        [SerializeField] private Value<float> shipAngleX;
        [SerializeField] private Value<float> shipAngleZ;
        private Vector2 shipAngle => new Vector2(shipAngleX.value, shipAngleZ.value);
        [SerializeField] private RectTransform gyroscopeRings;
        [SerializeField] private TextMeshProUGUI gyroscopeAngle;

        [Space(1f)]
        [Header("Others")]
        [SerializeField]
        private GameObject deathScreen;

        private void Update()
        {
            ClockUpdate();
            GyroscopeUpdate();
        }

        public void DeathScreen()
        {
            deathScreen.SetActive(true);
        }

        private void ClockUpdate()
        {
            if (timeValue >= .5f)
            {
                if (timeClock.fillClockwise)
                    timeClock.fillClockwise = false;

                timeClock.fillAmount = 1 - ((timeValue - .5f) / .5f);
            }
            else
            {
                if (!timeClock.fillClockwise)
                    timeClock.fillClockwise = true;

                timeClock.fillAmount = (timeValue) / .5f;
            }

            weekDayText.text = weekDay.ToString();
        }

        private void GyroscopeUpdate()
        {
            gyroscopeRings.eulerAngles = new Vector3(shipAngle.x, 0, shipAngle.y);
            gyroscopeAngle.text = $"X: {shipAngle.x:0}° Z: {shipAngle.y:0}°";
        }
    }
}
