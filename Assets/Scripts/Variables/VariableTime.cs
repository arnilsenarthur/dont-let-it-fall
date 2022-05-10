using UnityEngine;

namespace DontLetItFall.Variables
{
    [CreateAssetMenu(menuName = "DLIF/Variables/Time", fileName = "Time" , order = 2)]
    public class VariableTime : VariableFloat
    {
        public int minutesPrecision = 1;

        public override string ToString()
        {
            int minutes = Mathf.FloorToInt(((float)value * 60 * 24)/minutesPrecision) * minutesPrecision;

            int hours = minutes / 60;
            minutes = minutes % 60;

            return hours.ToString("00") + ":" + minutes.ToString("00");
        }
    }
}