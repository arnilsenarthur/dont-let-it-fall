using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DontLetItFall.Variables;

using TMPro;

namespace DontLetItFall.UI
{
    public class VariableDisplay : MonoBehaviour
    {
        private TMP_Text textComponent;

        public Variable variable;

        public string text
        {
            get => textComponent.text;
            set => textComponent.text = value;
        }

        public void OnEnable()
        {
            textComponent = GetComponent<TMP_Text>();

            variable.OnChange += ReloadText;
        }

        public void OnDisable()
        {
            variable.OnChange -= ReloadText;
        }

        public void ReloadText()
        {
            text = variable.ToString();
        }

    }
}