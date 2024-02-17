using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TextAndIconDisplay : MonoBehaviour
    {
        public RectTransform icon;
        public TMP_Text textComponent;

        public float offsetX;

        public void SetText(string text)
        {
            textComponent.text = text;
            
            float preferredWidth = textComponent.GetPreferredValues().x;

            float coinIconX = textComponent.transform.position.x - preferredWidth - offsetX;
            Vector3 position = icon.position;
            position = new Vector3(coinIconX, position.y, position.z);
            icon.position = position;
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                SetText(textComponent.text);
            }
        }
    }
}