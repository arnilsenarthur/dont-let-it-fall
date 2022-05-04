using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DontLetItFall.InputSystem
{
    #region Input Manager
    [CreateAssetMenu(fileName = "InputManager", menuName = "DLIF/InputManager", order = 0)]
    public class InputManager : ScriptableObject
    {
        public Binding[] bindings;

        public int GetIndexForBinding(string name)
        {
            for (int i = 0; i < bindings.Length; i++)
            {
                if (bindings[i].name == name)
                    return i;
            }

            return -1;
        }

        public Binding GetBinding(string name)
        {
            for (int i = 0; i < bindings.Length; i++)
            {
                if (bindings[i].name == name)
                    return bindings[i];
            }

            return null;
        }
    }
    #endregion

    #region Structs
    [System.Serializable]
    public class Binding
    {
        public string unlocalizedName;
        public string name;
        public BindingValue value;
    }

    [System.Serializable]
    public class BindingValue
    {
        public BindingType type;

        [SerializeField]
        private string stringValue;

        [SerializeField]
        private KeyCode keyValue;

        [SerializeField]
        private int intValue;

        [SerializeField]
        private MouseInputEnum mouseInputValue;

        public string ToSpriteIcon()
        {
            return ToSpriteIcon(GetUnlocalizedName());
        }

        public static string ToSpriteIcon(string name)
        {
            return "<sprite=\"bindings\" name=\"" + name + "\">";
        }

        public string GetUnlocalizedName()
        {
            switch (type)
            {
                case BindingType.Key:
                    return "key_" + keyValue.ToString().ToLowerInvariant();
                case BindingType.Button:
                    return "button_" + stringValue.ToLowerInvariant();
                case BindingType.MouseButton:
                    return "mouse_button_" + intValue.ToString().ToLowerInvariant();
                case BindingType.Axis:
                    return "axis_" + stringValue.ToLowerInvariant();
                case BindingType.Mouse:
                    switch (mouseInputValue)
                    {
                        case MouseInputEnum.X:
                            return "mouse_pos_x";
                        case MouseInputEnum.Y:
                            return "mouse_pos_y";
                        case MouseInputEnum.ScrollWheel:
                            return "mouse_scroll";
                        default:
                            return "???";

                    }
                case BindingType.Touch:
                    return "touch_" + intValue;

                default:
                    return "???";
            }
        }

        public float GetValue(BindingMode mode)
        {
            switch (type)
            {
                case BindingType.Key:
                    {
                        switch (mode)
                        {
                            case BindingMode.Default:
                                return Input.GetKey(keyValue) ? 1 : 0;
                            case BindingMode.Down:
                                return Input.GetKeyDown(keyValue) ? 1 : 0;
                            case BindingMode.Up:
                                return Input.GetKeyUp(keyValue) ? 1 : 0;
                            default:
                                return 0;
                        }
                    }
                case BindingType.Button:
                    switch (mode)
                    {
                        case BindingMode.Default:
                            return Input.GetButton(stringValue) ? 1 : 0;
                        case BindingMode.Down:
                            return Input.GetButtonDown(stringValue) ? 1 : 0;
                        case BindingMode.Up:
                            return Input.GetButtonUp(stringValue) ? 1 : 0;
                        default:
                            return 0;
                    }
                case BindingType.Axis:
                    switch (mode)
                    {
                        case BindingMode.Default:
                            return Input.GetAxis(stringValue);
                        case BindingMode.Raw:
                            return Input.GetAxisRaw(stringValue);
                        default:
                            return 0;
                    }
                case BindingType.MouseButton:
                    switch (mode)
                    {
                        case BindingMode.Default:
                            return Input.GetMouseButton(intValue) ? 1 : 0;
                        case BindingMode.Down:
                            return Input.GetMouseButtonDown(intValue) ? 1 : 0;
                        case BindingMode.Up:
                            return Input.GetMouseButtonUp(intValue) ? 1 : 0;
                        default:
                            return 0;
                    }

                case BindingType.Mouse:
                    switch (mouseInputValue)
                    {
                        case MouseInputEnum.X:
                            return Input.mousePosition.x;
                        case MouseInputEnum.Y:
                            return Input.mousePosition.y;
                        case MouseInputEnum.ScrollWheel:
                            return Input.GetAxis("Mouse ScrollWheel");
                        default:
                            return 0;
                    }

                case BindingType.Touch:
                    return Input.GetTouch(intValue).phase == TouchPhase.Began ? 1 : 0;
                default:
                    return 0;
            }
        }

    }
    #endregion

    #region Enums
    public enum BindingMode
    {
        //General
        Default,

        //Button and Key
        Down,
        Up,

        //Axis
        Raw
    }

    public enum BindingType
    {
        Key,
        Button,
        Axis,
        MouseButton,
        Mouse,
        Touch,
    }

    public enum MouseInputEnum
    {
        X,
        Y,
        ScrollWheel
    }
    #endregion

    #region Editor
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(BindingValue))]
    public class BindingValueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var type = property.FindPropertyRelative("type");
            var stringValue = property.FindPropertyRelative("stringValue");
            var keyValue = property.FindPropertyRelative("keyValue");
            var intValue = property.FindPropertyRelative("intValue");
            var mouseInputValue = property.FindPropertyRelative("mouseInputValue");

            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, type);

            //add new space
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            switch ((BindingType)type.enumValueIndex)
            {
                case BindingType.Key:
                    EditorGUI.PropertyField(position, keyValue, new GUIContent("Key Code"));
                    break;
                case BindingType.Button:
                    EditorGUI.PropertyField(position, stringValue, new GUIContent("Button Name"));
                    break;
                case BindingType.Axis:
                    EditorGUI.PropertyField(position, stringValue, new GUIContent("Axis Name"));
                    break;
                case BindingType.MouseButton:
                    EditorGUI.PropertyField(position, intValue, new GUIContent("Mouse Button"));
                    break;
                case BindingType.Touch:
                    EditorGUI.PropertyField(position, intValue, new GUIContent("Touch Index"));
                    break;
                case BindingType.Mouse:
                    EditorGUI.PropertyField(position, mouseInputValue, new GUIContent("Mouse Input"));
                    break;
            }

            EditorGUI.EndProperty();

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            return EditorGUIUtility.singleLineHeight * 2;
        }
    }
#endif
    #endregion
}