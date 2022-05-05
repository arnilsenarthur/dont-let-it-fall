using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System;
#endif

namespace DontLetItFall.Variables
{
    public abstract class Variable : ScriptableObject
    {
        protected static Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        public OnChangeEvent OnChange;

        public static Variable GetVariable(string key)
        {
            if (variables.ContainsKey(key))
                return variables[key];
            else
                return null;
        }

        public abstract void SetValue(object value);
    }


    public class VariableBase<T> : Variable
    {
        public T Value
        {
            set
            {
                if(this.value != null && this.value.Equals(value))
                    return;
                    
                this.value = value;
                if (OnChange != null) OnChange.Invoke();
            }
            get => value;
        }

        [SerializeField]
        protected T value;

        public string key;

        private void OnEnable()
        {
            if (key == null)
                return;

            variables[key] = this;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override void SetValue(object value)
        {
            this.value = (T)value;
        }
    }

    public delegate void OnChangeEvent();

    #region Ready Only Field
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                   SerializedProperty property,
                                   GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
    #endregion
}