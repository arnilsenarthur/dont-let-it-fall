using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace DontLetItFall.InputSystem
{
    #region Attribute
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class BindingAttribute : System.Attribute
    {
        public readonly string name;
        public readonly BindingMode mode;
        public readonly bool inverse;

        public BindingAttribute(BindingMode mode, bool inverse = false)
        {
            this.name = null;
            this.mode = mode;
            this.inverse = inverse;
        }

        public BindingAttribute(string name = null, BindingMode mode = BindingMode.Default, bool inverse = false)
        {
            this.name = name;
            this.mode = mode;
            this.inverse = inverse;
        }
    }
    #endregion

    #region Enums
    public enum FieldBindingType
    {
        Float,
        Bool,
        Vector2,
        Vector3,
        Vector4
    }
    #endregion

    #region Structs And Classes
    [Serializable]
    public struct FieldBinding
    {
        [HideInInspector]
        public FieldInfo field;
        public BindingMode mode;
        public bool inverse;
        public int index;
        public FieldBindingType type;
        public int propIndex;
    }

    [Serializable]
    public class InputInterface
    {
        public InputManager manager;
        private FieldBinding[] bindings;

        public void Update()
        {
            if (manager == null)
                return;

            if (bindings == null)
                CreateBindings();

            foreach (var binding in bindings)
            {

                if (binding.field == null)
                {
                    bindings = null;
                    return;
                }

                float value = manager.bindings[binding.index].value.GetValue(binding.mode);

                if (binding.inverse)
                {
                    value = -value;
                }

                switch (binding.type)
                {
                    case FieldBindingType.Float:
                        binding.field.SetValue(this, value);
                        break;
                    case FieldBindingType.Bool:
                        binding.field.SetValue(this, value > 0);
                        break;
                    case FieldBindingType.Vector2:
                        {
                            Vector2 v = (Vector2)binding.field.GetValue(this);
                            v[binding.propIndex] = value;
                            binding.field.SetValue(this, v);
                        }
                        break;
                    case FieldBindingType.Vector3:
                        {
                            Vector3 v = (Vector3)binding.field.GetValue(this);
                            v[binding.propIndex] = value;
                            binding.field.SetValue(this, v);
                        }
                        break;
                    case FieldBindingType.Vector4:
                        {
                            Vector4 v = (Vector4)binding.field.GetValue(this);
                            v[binding.propIndex] = value;
                            binding.field.SetValue(this, v);
                        }
                        break;
                }
            }
        }

        private string ValidateType(FieldInfo field, int number, ref FieldBindingType type)
        {
            if (number == 1)
            {
                if (field.FieldType == typeof(bool))
                {
                    type = FieldBindingType.Bool;
                    return null;
                }

                if (field.FieldType == typeof(float))
                {
                    type = FieldBindingType.Float;
                    return null;
                }

                return $"Binding field {field.Name} must be a bool or a float. Binding will be ignored!";
            }

            if (number == 2)
            {
                if (field.FieldType == typeof(Vector2))
                {
                    type = FieldBindingType.Vector2;
                    return null;
                }

                if (field.FieldType == typeof(Vector3))
                {
                    type = FieldBindingType.Vector3;
                    return null;
                }

                if (field.FieldType == typeof(Vector4))
                {
                    type = FieldBindingType.Vector4;
                    return null;
                }

                return $"Binding field {field.Name} must be a Vector2, a Vector3 or a Vector4. Binding will be ignored!";
            }

            if (number == 3)
            {
                if (field.FieldType == typeof(Vector3))
                {
                    type = FieldBindingType.Vector3;
                    return null;
                }

                if (field.FieldType == typeof(Vector4))
                {
                    type = FieldBindingType.Vector4;
                    return null;
                }

                return $"Binding field {field.Name} must be a Vector4 or a Vector3. Binding will be ignored!";
            }

            if (number == 4)
            {
                if (field.FieldType == typeof(Vector4))
                {
                    type = FieldBindingType.Vector4;
                    return null;
                }

                return $"Binding field {field.Name} must be a Vector4. Binding will be ignored!";
            }

            return $"Binding field {field.Name} has more than 4 bindings. Binding will be ignored!";
        }

        private void CreateBindings()
        {
            var fields = GetType().GetFields();

            List<FieldBinding> bindings = new List<FieldBinding>();

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(BindingAttribute), true);
                if (attributes.Length == 0)
                    continue;

                FieldBindingType type = FieldBindingType.Float;
                string typeValidation = ValidateType(field, attributes.Length, ref type);
                if (typeValidation != null)
                {
                    Debug.LogWarning(typeValidation);
                    continue;
                }

                int i = attributes.Length == 1 ? -1 : 0;
                foreach (BindingAttribute attribute in attributes)
                {
                    string name = attribute.name;
                    if (string.IsNullOrEmpty(name))
                        name = field.Name;

                    int index = manager == null ? -1 : manager.GetIndexForBinding(name);
                    if (index != -1)
                        bindings.Add(new FieldBinding() { field = field, mode = attribute.mode, index = index, type = type, propIndex = i, inverse = attribute.inverse });
                    else
                        Debug.LogWarning($"Binding not found: {name}. Binding will be ignored!");
                    i++;
                }
            }

            this.bindings = bindings.ToArray();
        }
    }
    #endregion
}