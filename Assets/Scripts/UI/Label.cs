using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;
using DontLetItFall.Data;
using DontLetItFall.Variables;

namespace DontLetItFall.UI
{
    public class Label : MonoBehaviour
    {
        static readonly Regex translationPattern = new Regex(@"\[([^\}]+)\]", RegexOptions.Compiled);
        static readonly Regex variablesPattern = new Regex(@"\{([^\}]+)\}", RegexOptions.Compiled);

        private TMP_Text textComponent;
        private static string[] argsCache = new string[16];
        private List<Variable> usedVariables = new List<Variable>();


        public string text
        {
            get => textComponent.text;
            set => textComponent.text = value;
        }

        private string initialText = null;

        public void OnEnable()
        {
            textComponent = GetComponent<TMP_Text>();
            initialText = text;
            ReloadText();
        }

        public void OnDisable()
        {
            text = initialText;

            /*
            foreach (Match match in variablesPattern.Matches(text))
            {
                string key = match.Groups[1].Value;
                Variable.Variable v = Variable.Variable.GetVariable(key);
                v.OnChange -= ReloadText;
            }
            */
            
            foreach(Variable v in usedVariables)
                v.OnChange -= ReloadText;
            usedVariables.Clear();

            LanguageManager.Instance.onChangeLanguage.RemoveListener(ReloadText);
        }

        public void ReloadText()
        {
            usedVariables.Clear();

            LanguageManager.Instance.onChangeLanguage.RemoveListener(ReloadText);
            LanguageManager.Instance.onChangeLanguage.AddListener(ReloadText);

            int index = -1;
            text = ParseLevel(ref initialText, ref index);
        }


        public string ParseLevel(ref string text, ref int index, int type = 0)
        {
            string cache = "";
            int argIndex = 0;

            while (index + 1 < text.Length)
            {
                index++;
                char c = text[index];

                if (type == 1)
                {
                    if (c == '[')
                    {
                        argsCache[argIndex] = ParseLevel(ref text, ref index, 1);
                        argIndex++;
                    }
                    else if (c == '{')
                    {
                        argsCache[argIndex] = ParseLevel(ref text, ref index, 2);
                        argIndex++;
                    }
                    else if (c == '%')
                    {
                        argsCache[argIndex] = ParseLevel(ref text, ref index, 3);
                        argIndex++;
                    }
                    else if (c == ']' || c == '}' || c == '%')
                        break;
                    else
                        cache += c;
                }
                else
                {
                    if (c == '[')
                        cache += ParseLevel(ref text, ref index, 1);
                    else if (c == '{')
                        cache += ParseLevel(ref text, ref index, 2);
                    else if (c == '%' && type != 3)
                        cache += ParseLevel(ref text, ref index, 3);
                    else if (c == ']' || c == '}' || c == '%')
                        break;
                    else
                        cache += c;
                }

            }

            if(type == 1)
                return LanguageManager.Instance.Localize(cache.Trim(),argsCache);
            else if(type == 2)
            {
                Variable v = Variable.GetVariable(cache.Trim());
                if(v == null)
                    return cache + "???";

                v.OnChange -= ReloadText;
                v.OnChange += ReloadText;
                usedVariables.Add(v);
                return v.ToString();
            }

            return cache;
        }


#if UNITY_EDITOR
        [MenuItem("GameObject/UI/DLIF - Label", false, 0)]
        public static void New(MenuCommand command)
        {
            GameObject o = new GameObject();

            GameObject parent = command.context as GameObject;

            o.transform.parent = parent == null ? null : parent.transform;
            o.transform.name = "Label";
            Selection.activeTransform = o.transform;

            //Add components
            RectTransform transform = o.AddComponent<RectTransform>();
            o.AddComponent<CanvasRenderer>();
            TextMeshProUGUI tx = o.AddComponent<TextMeshProUGUI>();


            Label label = o.AddComponent<Label>();
            tx.text = "New Label";

            transform.anchoredPosition = Vector3.zero;
        }
#endif
    }
}