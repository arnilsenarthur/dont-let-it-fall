using UnityEngine;

namespace DontLetItFall.Variables
{
    [CreateAssetMenu(menuName = "DLIF/Variables/Time", fileName = "Time" , order = 2)]
    public class VariableTime : VariableFloat
    {
        public override string ToString()
        {
            int minutes = Mathf.FloorToInt(value * 60 * 24);

            int hours = minutes / 60;
            minutes = minutes % 60;

            return hours.ToString("00") + ":" + minutes.ToString("00");
        }
    }
}