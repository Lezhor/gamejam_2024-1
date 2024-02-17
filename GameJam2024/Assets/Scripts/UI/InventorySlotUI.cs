using GameLogic.world;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        [Header("Resizing")] 
        [Range(1f, 2f)]
        public float sizeIncreaseWhenSelected = 1.1f;
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

        public void SetLabel(int number)
        {
            textLabel.SetText($"{number}");
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
            frameImage.sprite = selected ? frameSelectedSprite : frameUnselectedSprite;
            SetTransparency(schematicImage, selected ? selectedAlpha : unselectedAlpha);
            SetSize(selected ? sizeIncreaseWhenSelected : 1f);
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

    }
}
