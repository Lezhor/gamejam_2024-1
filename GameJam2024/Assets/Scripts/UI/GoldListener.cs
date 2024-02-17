using System;
using GameLogic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextAndIconDisplay))]
    public class GoldListener : MonoBehaviour
    {

        private TextAndIconDisplay _goldDisplay;
        
        

        private void Start()
        {
            _goldDisplay = GetComponent<TextAndIconDisplay>();
            GameManager.Instance.PlayerScript.PlayerInventory.OnGoldValueChanged += UpdateGoldValue;
            _goldDisplay.SetText(FormatInt(GameManager.Instance.PlayerScript.PlayerInventory.Gold));
        }

        private void UpdateGoldValue(int oldValue, int newValue)
        {
            _goldDisplay.SetText(FormatInt(newValue));
        }

        private String FormatInt(int value)
        {
            return value.ToString("### ### ###");
        }
    }
}