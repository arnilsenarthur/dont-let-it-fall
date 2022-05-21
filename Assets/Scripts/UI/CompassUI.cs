using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Variables;
using UnityEngine;

namespace DontLetItFall
{
    public class CompassUI : MonoBehaviour
    {
        [SerializeField] private Value<float> shipAngleY;
        [SerializeField] private RectTransform compassIcon;
        
        void Update()
        {
            compassIcon.localEulerAngles = new Vector3(0, 0, shipAngleY.value);
        }
    }
}
