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

        private Transform _canvas;
        private Transform _box;

        private void Start() {
            _image = content.GetComponent<Image>();
            _canvas = transform.parent;
            _box = _canvas.parent;
        }

        private void Update()
        {
            _canvas.position = _box.position + new Vector3(0, 1f, 0);
            
            transform.LookAt(Camera.main.transform);
            
            content.localScale = new Vector3(value, 1, 1);
            _image.color = Color.Lerp(Color.red, Color.green, value);
        }
    }
}