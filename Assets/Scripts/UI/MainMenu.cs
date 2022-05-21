using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Background")]
        [SerializeField]
        private RectTransform _canvas;
        [SerializeField]
        private RectTransform _background;
        [SerializeField]
        private RectTransform _backgroundContainer;
        
        [SerializeField]
        private Vector2 _safeBackgroundArea = new Vector2(150, 300);
        private float _backgroundX => _background.rect.width;
        private float _canvasX => _canvas.sizeDelta.x;
        private float _safeAreaX => ((_backgroundX - _canvasX) / 2) - _safeBackgroundArea.x;
        private Vector3 _containerPosition => _backgroundContainer.localPosition;
        private Vector2 _backgroundAbs => new Vector2(Mathf.Abs(_background.localPosition.x), Mathf.Abs(_background.localPosition.y));
        
        private Vector2 _backgroundLimit;
        
        [SerializeField]
        private Sprite[] _backgroundSprites;

        [SerializeField] [Range(0f,20f)]
        private float _backgroundSpeed = 1;
        [SerializeField] [Range(0f,20f)]
        private float _mouseSpeed = 1;
        
        [SerializeField]
        private bool _backgroundMovingLeft = true;
        
        [Space(1f)]
        [Header("Logo")]
        [SerializeField]
        private Image _logo;
        [SerializeField]
        private RectTransform _logoContainer;
        
        [SerializeField]
        private Sprite[] _logoSprites;
        
        [SerializeField]
        private int _logoTouchs = 0;
        [SerializeField]
        private int _logoTouchsMax = 20;

        [SerializeField] 
        private int _logoFalls = 0;

        private bool _isFalling = false;

        void Start()
        {
            _background.GetComponent<Image>().sprite = _backgroundSprites[Random.Range(0, _backgroundSprites.Length)];

            _backgroundLimit = new Vector2(
                _containerPosition.x - _safeAreaX,
                _containerPosition.x + _safeAreaX);

            _backgroundContainer.localPosition = new Vector3(_backgroundLimit.x, _containerPosition.y, 0);
        }

        void Update()
        {
            MoveBackground();
            MouseBackground();
        }

        public void LogoTouch()
        {
            _logoTouchs++;
            if (_logoTouchs % 2 == 0)
            {
                _logoContainer.eulerAngles = new Vector3(0, 0, Random.Range(-5, 5.1f));
            }

            if (_logoTouchs >= _logoTouchsMax && !_isFalling)
            {
                _logoTouchs = 0;
                _isFalling = true;
                _logo.GetComponent<Animator>().SetBool("Fall", true);
                StartCoroutine(LogoFallOff());
                
                if(_logoFalls == _logoSprites.Length-1)
                    _logoFalls = 0;
                else
                    _logoFalls++;
            }
        }

        private void MouseBackground()
        {
            float mouseX = Input.GetAxis("Mouse X") * _mouseSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSpeed;

            if ((_backgroundAbs.x + mouseX)+5 < _safeBackgroundArea.x)
                _background.position += new Vector3(mouseX, 0, 0);
            
            if ((_backgroundAbs.y + mouseY)+5 < _safeBackgroundArea.y)
                _background.position += new Vector3(0, mouseY, 0);
        }

        private void MoveBackground()
        {
            if (_backgroundMovingLeft)
            {
                _backgroundContainer.position += new Vector3(_backgroundSpeed * Time.deltaTime, 0, 0);

                if (_backgroundContainer.localPosition.x >= _backgroundLimit.y)
                    _backgroundMovingLeft = false;
            }
            else
            {
                _backgroundContainer.position -= new Vector3(_backgroundSpeed * Time.deltaTime, 0, 0);
                if (_backgroundContainer.localPosition.x <= _backgroundLimit.x)
                    _backgroundMovingLeft = true;
            }
        }

        private IEnumerator LogoFallOff()
        {
            while (!_logo.GetComponent<Animator>().IsInTransition(0))
            {
                yield return null;
            }
            
            _logo.GetComponent<Animator>().SetBool("Fall", false);

            yield return new WaitForSeconds(1f);
            _logo.sprite = _logoSprites[_logoFalls];
            _isFalling = false;
        }
    }
}
