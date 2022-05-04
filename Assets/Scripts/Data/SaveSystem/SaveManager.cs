using UnityEngine;
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