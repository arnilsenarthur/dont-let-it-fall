using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

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
        public string currentLanguage = "ptBR";
        public Language[] languages;
        public UnityEvent onChangeLanguage;
        #endregion

        #region Private Fields
        private Dictionary<string, Language> parsedLanguages = new Dictionary<string, Language>();
        #endregion

        private void Init()
        {
            foreach (Language language in languages)
            {
                language.UpdateEntries();
                parsedLanguages.Add(language.unlocalizedName, language);
            }
        }

        public string Localize(string key, params string[] args)
        {
            return parsedLanguages[currentLanguage].Localize(key, args);
        }
    }
}