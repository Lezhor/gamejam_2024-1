using System;
using GameLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class SpecialSlotUI : InventorySlotUI, IPointerClickHandler
    {
        [Header("Foreground")]
        public Image foreground;
        public Sprite foregroundSprite;

        private UIManager _uiManager;

        private void Start()
        {
            _uiManager = GameManager.Instance.UIManager;
        }

        protected override void Redraw()
        {
            base.Redraw();
            if (schematicImage.sprite == nullSchematicSprite)
            {
                foreground.sprite = foregroundSprite;
                SetTransparency(foreground, 1f);
            }
            else
            {
                foreground.sprite = null;
                SetTransparency(foreground, 0f);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_uiManager.GameActive && schematicImage.sprite == nullSchematicSprite)
            {
                _uiManager.ShowSpecialSlotShop();
            }
            else
            {
                base.OnPointerClick(eventData);
            }
        }
    }
}