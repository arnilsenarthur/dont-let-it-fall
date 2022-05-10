using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DontLetItFall.Variables
{
    [System.Serializable]
    public class Value<T>
    {
        [SerializeField]
        private T _value;

        [SerializeField]
        private VariableBase<T> variable;

        public T value
        {
            get
            {
                try
                {
                    return variable.Value;
                }
                catch
                {
                    return _value;
                }
            }

            set
            {
                if (variable != null)
                {
                    variable.Value = value;
                }
                else
                {
                    _value = value;
                }
            }
        }

        public Value() { }
        public Value(T value)
        {
            _value = value;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Value<>))]
    public class ValueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect pos = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(pos, property.FindPropertyRelative("variable"), label);

            pos.y += EditorGUIUtility.singleLineHeight + 2;

            if (property.FindPropertyRelative("variable").objectReferenceValue == null)
                EditorGUI.PropertyField(pos, property.FindPropertyRelative("_value"), new GUIContent(" "));

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.FindPropertyRelative("variable").objectReferenceValue == null)
                return EditorGUIUtility.singleLineHeight * 2 + 2;
            else
                return EditorGUIUtility.singleLineHeight + 2;
        }
    }
#endif
}