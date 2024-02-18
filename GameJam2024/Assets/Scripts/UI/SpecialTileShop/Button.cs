using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.SpecialTileShop
{
    public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action OnButtonClicked;
        
        private bool _mouseHovers;
        [Header("Stats")]
        [Range(1f, 2f)]
        public float increaseSizeWhenHover = 1.05f;
        public float alphaIfCantBeClicked = .5f;

        [Header("Components")] 
        public Image buttonBackground;
        public TMP_Text buttonText;

        private bool _canBeClicked = true;

        public bool CanBeClicked
        {
            get => _canBeClicked;
            set
            {
                _canBeClicked = value;
                Redraw();
            }
        }

        private void Redraw()
        {
            if (CanBeClicked)
            {
                SetScale(_mouseHovers ? increaseSizeWhenHover : 1f);
                SetTransparency(buttonBackground, 1f);
                SetTransparency(buttonText, 1f);
            }
            else
            {
                SetScale(1f);
                SetTransparency(buttonBackground, alphaIfCantBeClicked);
                SetTransparency(buttonText, alphaIfCantBeClicked);
            }
        }

        private void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }
        
        private void SetTransparency(Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    
        private void SetTransparency(TMP_Text text, float alpha)
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseHovers = true;
            Redraw();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseHovers = false;
            Redraw();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (CanBeClicked)
            {
                OnButtonClicked?.Invoke();
            }
        }
    }
}
