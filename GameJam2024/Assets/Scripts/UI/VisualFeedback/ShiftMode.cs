using GameLogic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UI.VisualFeedback
{
    public class ShiftMode : MonoBehaviour
    {
        [Header("Scrolling Feedback")] [SerializeField]
        private Tilemap scrollingFeedbackTilemap;

        [SerializeField] private float maxAlpha = .4f;

        [Header("Fade")] [SerializeField] private float fadeTime = 1f;

        private float _lastPressTime;
        private float _currentValue;

        private bool _shiftPressed;

        private UIManager _uiManager;

        private void Start()
        {
            _uiManager = GameManager.Instance.UIManager;
            UpdateEverything(_currentValue);
        }

        private void Update()
        {
            // TODO - Can be toggled
            if (_uiManager.GameActive && Input.GetKeyDown(KeyCode.LeftShift))
            {
                _lastPressTime = Time.time;
                _shiftPressed = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _lastPressTime = Time.time;
                _shiftPressed = false;
            }

            float lastFrameValue = _currentValue;

            float valueChange = (Time.time - _lastPressTime) * Time.deltaTime / fadeTime;
            _currentValue = Mathf.Clamp(_currentValue + (_shiftPressed ? valueChange : -valueChange), 0, 1);

            if (_currentValue != lastFrameValue)
            {
                UpdateEverything(_currentValue);
            }
        }

        private void UpdateEverything(float value)
        {
            SetTransparency(scrollingFeedbackTilemap, Mathf.Lerp(0, maxAlpha, value));
        }

        private void SetTransparency(Tilemap tilemap, float alpha)
        {
            Color c = tilemap.color;
            c.a = alpha;
            tilemap.color = c;
        }
    }
}