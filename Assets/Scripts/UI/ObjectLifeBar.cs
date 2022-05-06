using UnityEngine;
using UnityEngine.UI;
using DontLetItFall.Variables;

namespace DontLetItFall.UI
{
    public class ObjectLifeBar : MonoBehaviour
    {
        [Range(0, 1)]
        public float value;

        public RectTransform content;
        
        private Image _image;

        private void Start() {
            _image = content.GetComponent<Image>();
        }

        private void Update()
        {
            content.localScale = new Vector3(value, 1, 1);
            _image.color = Color.Lerp(Color.red, Color.green, value);
        }
    }
}