using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DontLetItFall.Data
{
    [CreateAssetMenu(fileName = "LanguageManager", menuName = "DLIF/LanguageManager", order = 0)]
    public class LanguageManager : ScriptableObject
    {
        #region Static Fields
        public static LanguageManager Instance
        {
            get
            {
                if (_instanceCache == null)
                {
                    _instanceCache = Resources.Load<LanguageManager>("Data/LanguageManager");
                    _instanceCache.Init();
                }

                return _instanceCache;
            }
        }

        private static LanguageManager _instanceCache;
        #endregion

        #region Public Fields
        public Language[] languages;
        public UnityEvent onChangeLanguage;
        #endregion

        #region Private Fields
        private Dictionary<string, Language> parsedLanguages = new Dictionary<string, Language>();
        #endregion

        public void Init()
        {
            foreach (Language language in languages)
            {
                language.UpdateEntries();
                parsedLanguages[language.unlocalizedName] = language;
            }
        }

        public string Localize(string key, params string[] args)
        {
            Language lang;
            if (parsedLanguages.TryGetValue(SaveManager.Data.language, out lang))
                return lang.Localize(key, args);
            else
                return key + "???";
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LanguageManager))]
    public class LanguageManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update Languages"))
            {
                LanguageManager languageManager = (LanguageManager)target;
                languageManager.Init();
            }
        }
    }
#endif
}

