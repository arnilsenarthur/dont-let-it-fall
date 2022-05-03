using UnityEngine;
using UnityEngine.Events;

namespace DontLetItFall
{
    public class GameController : MonoBehaviour 
    {
        #region Events
        public UnityEvent OnPause;
        public UnityEvent OnResume;
        #endregion

        public bool paused = false;

        private float testA = 0;
        private float testB = 0;

        public void Pause()
        {
            paused = true;
            Time.timeScale = 0;
            OnPause.Invoke();
        }

        public void Resume()
        {
            paused = false;
            Time.timeScale = 1;
            OnResume.Invoke();
        }

        private void Update() {
            testA += Time.deltaTime;
            testB += Time.unscaledDeltaTime;
        }

        private void OnGUI() {
            if (paused) {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, 20, 100, 20), "Resume")) {
                    Resume();
                }
            }
            else {
                if (GUI.Button(new Rect(Screen.width / 2 - 50, 20, 100, 20), "Pause")) {
                    Pause();
                }
            }

            GUI.Label(new Rect(Screen.width / 2 - 250, 40, 500, 20), "Time.deltaTime: " + testA);
            GUI.Label(new Rect(Screen.width / 2 - 250, 60, 500, 20), "Time.unscaledDeltaTime: " + testB);
        }
    }
}