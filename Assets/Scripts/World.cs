using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using DontLetItFall.Variables;

namespace DontLetItFall
{
    public class World : MonoBehaviour
    {
        public static string[] weekyDays = { "sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday" };

        #region Inspector Variables
        public Value<float> worldTime;
        public Value<float> dayTime;
        public Value<int> dayCount;
        public Value<string> dayWeekName;
        #endregion

        #region Public Methods
        [Header("SETTINGS")]
        public float secondsPerDay = 20f;

        [Header("SKYBOX SETTINGS")]
        public AnimationCurve skySunSize;
        public AnimationCurve skySunSizeConvergence;
        public AnimationCurve skyAthmosphereThickness;
        public AnimationCurve skyExposure;

        [Header("EVENTS")]
        public UnityEvent<int> onDayChanged;

        public GameObject player;
        public GameObject deathScreen;
        #endregion

        private void Start()
        {
            worldTime.value = secondsPerDay/2f;
        }

        private void Update()
        {
            int last = (int)(worldTime.value / secondsPerDay);
            worldTime.value += Time.deltaTime;
            int now = (int)(worldTime.value / secondsPerDay);

            dayTime.value = (worldTime.value % secondsPerDay) / secondsPerDay;
            dayCount.value = now + 1;
            dayWeekName.value = weekyDays[now % 7];

            if (last != now)
                onDayChanged.Invoke(now);

            RenderSettings.skybox.SetFloat("_SunSize", skySunSize.Evaluate(dayTime.value));
            RenderSettings.skybox.SetFloat("_SunSizeConvergence", skySunSizeConvergence.Evaluate(dayTime.value));
            RenderSettings.skybox.SetFloat("_AtmosphereThickness", skyAthmosphereThickness.Evaluate(dayTime.value));
            RenderSettings.skybox.SetFloat("_Exposure", skyExposure.Evaluate(dayTime.value));
            DynamicGI.UpdateEnvironment();
        }

        #region Test Section
        private void FixedUpdate()
        {
            if (player.transform.position.y < -15f)
            {
                deathScreen.SetActive(true);
            }
        }

        public void PlayAgain()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        #endregion
    }
}