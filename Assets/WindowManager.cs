using System;
using System.Collections;
using System.Collections.Generic;
using DontLetItFall.Variables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DontLetItFall
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] 
        private VariableString _sceneName;
        
        [SerializeField]
        private GameObject[] _window;

        [SerializeField] 
        private int _value;

        [SerializeField] 
        private UnityEvent OnValueChanged;
        [SerializeField] 
        private UnityEvent OnSceneLoaded;
        [SerializeField] 
        private UnityEvent OnGameExit;

        public void ChangeWindow(int value)
        {
            OnValueChanged.Invoke();
            
            _window[_value].SetActive(false);
            _value = value;
            _window[_value].SetActive(true);
        }

        public void LoadScene(string sceneName)
        {
            OnSceneLoaded.Invoke();
            
            _sceneName.Value = sceneName;
            
            SceneManager.LoadScene("Loading");
        }

        public void ExitGame()
        {
            OnGameExit.Invoke();

            Application.Quit();
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach(var window in _window)
            {
                window.SetActive(false);
            }
            _window[_value].SetActive(true);
        }
#endif
    }
}
