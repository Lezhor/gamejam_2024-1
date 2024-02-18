using GameLogic;
using GameLogic.world;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.SpecialTileShop
{
    public class SpecialSlotShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [FormerlySerializedAs("sizeIncreaseWhenSelected")]
        [Header("Resizing")] 
        [Range(1f, 2f)]
        public float sizeIncrease = 1.1f;
        [Header("Schematic")]
        public Image schematicImage;
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

        private int _index;
        private SpecialSlotShop _shop;
        
        public void SetLabel(int number)
        {
            textLabel.SetText($"{number}");
        }

        public void Initialize(int index, TileData tile, SpecialSlotShop shop)
        {
            _index = index;
            _shop = shop;
            SetLabel(index + 1);
            _selected = false;
            _mouseHovers = false;
            UpdateSchematic(tile);
            Redraw();
        }

        public void UpdateSchematic(TileData tile)
        {
            if (tile == null)
            {
                schematicImage.sprite = null;
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
            SetSize((_selected || _mouseHovers ? sizeIncrease : 1f));
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

        public void OnPointerClick(PointerEventData eventData)
        {
            _shop.ItemSelected(_index);
        }
    }
}