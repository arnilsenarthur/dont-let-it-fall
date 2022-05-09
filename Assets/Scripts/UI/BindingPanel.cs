using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DontLetItFall.InputSystem;
using DontLetItFall.Utils;

namespace DontLetItFall.UI
{
    [System.Serializable]
    public class BindingGroup
    {
        public InputManager manager;
        public string title;
        public string[] bindings;
    }

    public class BindingPanel : MonoBehaviour
    {
        #region Public Fields
        [Header("PREFABS")]
        public GameObject bindingTitlePrefab;
        public GameObject bindingItemPrefab;
        [Header("DISPLAY")]
        public BindingGroup[] groups;
        #endregion

        private void OnDisable()
        {
            //Destroy all current items
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }
        
        private void OnEnable()
        {
            //Generate all bindings
            foreach (BindingGroup group in groups)
            {
                /*
                bindingTitlePrefab.SetActive(false);
                GameObject title = Instantiate(bindingTitlePrefab, transform);

                title.GetComponent<PrefabInfo>().objects[0].GetComponent<TextMeshProUGUI>().text = "[" + group.title + "]";
                
                title.SetActive(true);
                */

                foreach (string binding in group.bindings)
                {
                    bindingItemPrefab.SetActive(false);
                    GameObject item = Instantiate(bindingItemPrefab, transform);

                    PrefabInfo info = item.GetComponent<PrefabInfo>();
                    Binding b = group.manager.GetBinding(binding);

                    info.objects[0].GetComponent<TextMeshProUGUI>().text = "[" + b.unlocalizedName + "]";
                    info.objects[1].GetComponent<TextMeshProUGUI>().text = b.value.ToSpriteIcon();
                
                    item.SetActive(true);
                }            
            }

            string s = "";

            foreach(KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
            {
                s += "key_ " + code.ToString().ToLower() + "\n";
            }

            Debug.Log(s);
        }

        private void Update()
        {
            
        }
    }
}