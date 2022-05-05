using UnityEngine;
using DontLetItFall.Variables;

namespace DontLetItFall
{
    public class World : MonoBehaviour
    {
        public float secondsPerDay = 20f;

        #region Inspector Variables
        public Value<float> worldTime;
        public Value<float> dayTime;
        public Value<int> dayCount;
        #endregion

        public AnimationCurve skySunSize;
        public AnimationCurve skySunSizeConvergence;
        public AnimationCurve skyAthmosphereThickness;
        public AnimationCurve skyExposure;



        private void Update()
        {
            worldTime.value += Time.deltaTime;
            dayTime.value = (worldTime.value % secondsPerDay) / secondsPerDay;
            dayCount.value = (int)(worldTime.value / secondsPerDay) + 1;

            RenderSettings.skybox.SetFloat("_SunSize", skySunSize.Evaluate(dayTime.value));
            RenderSettings.skybox.SetFloat("_SunSizeConvergence", skySunSizeConvergence.Evaluate(dayTime.value));
            RenderSettings.skybox.SetFloat("_AtmosphereThickness", skyAthmosphereThickness.Evaluate(dayTime.value));
            RenderSettings.skybox.SetFloat("_Exposure", skyExposure.Evaluate(dayTime.value));

        }

    }
}