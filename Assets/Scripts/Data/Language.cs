using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DontLetItFall.Data
{
    [CreateAssetMenu(fileName = "Language", menuName = "DLIF/Language", order = 0)]
    public class Language : ScriptableObject
    {
        public string unlocalizedName;
        public string localizedName;

        [TextArea(50, 500)]
        public string entries;

        private Dictionary<string, string> parsedEntries = new Dictionary<string, string>();

        public void UpdateEntries()
        {
            parsedEntries.Clear();
            string[] lines = entries.Split('\n');
            foreach (string line in lines)
            {
                string[] entry = line.Split('=');
                if (entry.Length == 2)
                {
                    parsedEntries.Add(entry[0], entry[1]);
                }
            }

            Debug.Log($"[Languages] Loaded {parsedEntries.Count} entries for {localizedName}");
        }

        public string Localize(string key, params string[] args)
        {
            if (parsedEntries.ContainsKey(key))
            {
                return string.Format(parsedEntries[key], args);
            }

            return key + "???";
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Reload Entries"))
            {
                ((Language)target).UpdateEntries();
                LanguageManager.Instance.onChangeLanguage.Invoke();
            }
        }
    }
    #endif
}