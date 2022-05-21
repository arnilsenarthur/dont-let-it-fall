using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Variables;
using TMPro;
using UnityEngine;

namespace DontLetItFall
{
    public class GyroscopeUI : MonoBehaviour
    {
        [SerializeField] private Value<float> shipAngleX;
        [SerializeField] private Value<float> shipAngleZ;
        private Vector2 shipAngle => new Vector2(shipAngleX.value, shipAngleZ.value);
        [SerializeField] private RectTransform gyroscopeRings;
        [SerializeField] private TextMeshProUGUI gyroscopeAngle;

        private void Update()
        {
            gyroscopeRings.eulerAngles = new Vector3(shipAngle.x, 0, shipAngle.y);
            gyroscopeAngle.text = $"X: {shipAngle.x:0}° Z: {shipAngle.y:0}°";
        }
    }
}
