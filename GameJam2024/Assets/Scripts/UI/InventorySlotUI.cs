using System;
using GameLogic.world;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Resizing")] 
        [Range(1f, 2f)]
        public float sizeIncreaseWhenSelected = 1.1f;
        public float sizeIncreaseWhenMouseHover = 1.1f;
        [Header("Schematic")]
        public Image schematicImage;
        public Sprite nullSchematicSprite;
        [Range(0f, 1f)]
        public float selectedAlpha = 1f;
        [Range(0f, 1f)]
        public float unselectedAlpha = 0.6f;
        
        [Header("Frame")]
        public Image frameImage;
        public Sprite frameSelectedSprite;
        public Sprite frameUnselectedSprite;
        
        [Header("Text")]
        public TMP_Text textLabel;

        private bool _mouseHovers = false;
        private bool _selected = false;

        public void SetLabel(int number)
        {
            textLabel.SetText($"{number}");
        }

        public void UpdateSchematic(TileData tile)
        {
            if (tile == null)
            {
                schematicImage.sprite = nullSchematicSprite;
                SetTransparency(schematicImage, 0f);
            } else {
                schematicImage.sprite = tile.sprite;
                SetTransparency(schematicImage, 1f);
            }
        }

        public void UpdateSelectedState(bool selected)
        {
            _selected = selected;
            Redraw();
        }

        protected virtual void Redraw()
        {
            frameImage.sprite = _selected ? frameSelectedSprite : frameUnselectedSprite;
            SetTransparency(schematicImage, schematicImage.sprite == null ? 0f : (_selected ? selectedAlpha : unselectedAlpha));
            SetSize((_selected ? sizeIncreaseWhenSelected : 1f) * (_mouseHovers ? sizeIncreaseWhenMouseHover : 1f));
        }

        private void SetTransparency(Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        private void SetSize(float size)
        {
            transform.localScale = new Vector3(size, size, size);
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

    }
}
