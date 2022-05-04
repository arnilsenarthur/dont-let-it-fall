using UnityEngine;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DontLetItFall.Variables;
using System.Reflection;

namespace DontLetItFall.Data
{
    [System.Serializable]
    public class SaveData
    {
        public string language = "ptBR";

        public float masterVolume = 1f;
        public float musicVolume = 1f;
        public float sfxVolume = 1f;
    }

    [System.Serializable]
    public class VariableReference
    {
        public string field;
        public Variable variable;
    }

    [CreateAssetMenu(fileName = "SaveManager", menuName = "DLIF/SaveManager", order = 0)]
    public class SaveManager : ScriptableObject
    {
        #region Static Fields
        public static SaveData Data
        {
            get
            {
                return Instance.saveData;
            }
        }

        public static SaveManager Instance
        {
            get
            {
                if (_instanceCache == null)
                {
                    _instanceCache = Resources.Load<SaveManager>("Data/SaveManager");
                    _instanceCache.LoadData();
                }

                return _instanceCache;
            }
        }

        private static SaveManager _instanceCache;
        #endregion

        #region Public Fields
        public SaveData saveData
        {
            get
            {
                if (_saveData == null)
                    LoadData();

                return _saveData;
            }
        }

        public AudioMixer mixer;
        #endregion

        #region Private Fields
        [SerializeField]
        private SaveData _saveData;
        [SerializeField]
        private VariableReference[] _variableReferences;
        #endregion

        public void SaveData()
        {
            if (_saveData == null)
                return;

            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/save.data";

            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, _saveData);
            stream.Close();

            Debug.Log("[SaveSystem] Save Saved!");
        }

        public void LoadData()
        {
            string path = Application.persistentDataPath + "/save.data";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                SaveData data = formatter.Deserialize(stream) as SaveData;

                stream.Close();

                _saveData = data;

                Debug.Log("[SaveSystem] Save Loaded!");
            }
            else
            {
                _saveData = new SaveData();
                Debug.Log("[SaveSystem] Save file not found in " + path);
            }

            ApplyVariables();
        }

        public void ApplyVariables()
        {
            foreach (VariableReference reference in _variableReferences)
            {
                string fieldName = reference.field;
                FieldInfo field = typeof(SaveData).GetField(fieldName);
                reference.variable.SetValue(field.GetValue(_saveData));
            }

            _saveData.masterVolume = Mathf.Clamp(_saveData.masterVolume,0.0001f,1f);
            _saveData.musicVolume = Mathf.Clamp(_saveData.musicVolume,0.0001f,1f);
            _saveData.sfxVolume = Mathf.Clamp(_saveData.sfxVolume,0.0001f,1f);

            mixer.SetFloat("MasterVolume", Mathf.Log10(_saveData.masterVolume) * 20);
            mixer.SetFloat("MusicVolume", Mathf.Log10(_saveData.musicVolume) * 20);
            mixer.SetFloat("SFXVolume", Mathf.Log10(saveData.sfxVolume) * 20);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SaveManager))]
    public class SaveManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SaveManager manager = (SaveManager)target;

            if (GUILayout.Button("Save"))
            {
                manager.SaveData();
            }

            if (GUILayout.Button("Load"))
            {
                manager.LoadData();
            }

            if (GUILayout.Button("Apply Variables"))
            {
                manager.ApplyVariables();
            }
        }
    }
#endif
}