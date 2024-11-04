using System;
using GameLogic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GoldListener : MonoBehaviour
    {
        [SerializeField] private TMP_Text goldText;

        private float _currentValue;
        private int _displayedCurrentValue;
        private int _targetValue;

        [SerializeField]
        private float uiActualizationSpeed = 4;
        

        private void Start()
        {
            GameManager.Instance.PlayerScript.PlayerInventory.OnGoldValueChanged += UpdateGoldValue;
            
            _targetValue = GameManager.Instance.PlayerScript.PlayerInventory.Gold;
            _currentValue = _targetValue;
            
            goldText.text = FormatInt(_targetValue);
        }

        private void Update()
        {
            if (_displayedCurrentValue != _targetValue)
            {
                float difference = _targetValue - _currentValue;
                _currentValue += difference * uiActualizationSpeed * Time.deltaTime;
                _displayedCurrentValue = Mathf.RoundToInt(_currentValue);
                goldText.text = FormatInt(_displayedCurrentValue);
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
            String format = "###";
            if (value >= 1000)
            {
                format = "### ###";
            }

            if (value >= 1_000_000)
            {
                format = "### ### ###";
            }
            return value == 0 ? "0" : value.ToString(format);
        }
    }
}