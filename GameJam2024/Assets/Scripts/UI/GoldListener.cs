using System;
using GameLogic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextAndIconDisplay))]
    public class GoldListener : MonoBehaviour
    {

        private TextAndIconDisplay _goldDisplay;

        private float _currentValue;
        private int _displayedCurrentValue;
        private int _targetValue;

        [SerializeField]
        private float uiActualizationSpeed = 4;
        

        private void Start()
        {
            _goldDisplay = GetComponent<TextAndIconDisplay>();
            GameManager.Instance.PlayerScript.PlayerInventory.OnGoldValueChanged += UpdateGoldValue;
            
            _targetValue = GameManager.Instance.PlayerScript.PlayerInventory.Gold;
            _currentValue = _targetValue;
            
            _goldDisplay.SetText(FormatInt(_targetValue));
            
        }

        private void Update()
        {
            if (_displayedCurrentValue != _targetValue)
            {
                float difference = _targetValue - _currentValue;
                _currentValue += difference * uiActualizationSpeed * Time.deltaTime;
                _displayedCurrentValue = Mathf.RoundToInt(_currentValue);
                _goldDisplay.SetText(FormatInt(_displayedCurrentValue));
            }
            else
            {
                _currentValue = _displayedCurrentValue;
            }
        }

        private void UpdateGoldValue(int oldValue, int newValue)
        {
            _targetValue = newValue;
        }

        private String FormatInt(int value)
        {
            return value == 0 ? "0" : value.ToString("### ### ###");
        }
    }
}