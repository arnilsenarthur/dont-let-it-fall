using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DontLetItFall
{
    public class GyroscopeUI : MonoBehaviour
    {
        [SerializeField] private Vector2 shipAngle;
        [SerializeField] private RectTransform gyroscopeRings;
        [SerializeField] private TextMeshProUGUI gyroscopeAngle;

        private void Update()
        {
            gyroscopeRings.eulerAngles = new Vector3(shipAngle.x, 0, shipAngle.y);
            gyroscopeAngle.text = $"X: {shipAngle.x:0}° Z: {shipAngle.y:0}°";
        }
    }
}
