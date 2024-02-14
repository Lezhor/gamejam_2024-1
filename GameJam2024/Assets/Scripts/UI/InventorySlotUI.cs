using System;
using GameLogic.world;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventorySlotUI : MonoBehaviour
    {

        public Image schematic;
        public Image frame;
        public TMP_Text textLabel;

        public void SetLabel(int number)
        {
            textLabel.SetText($"{number}");
        }

        public void UpdateSchematic(TileData tile)
        {
            if (tile == null)
            {
                schematic.sprite = null;
                SetTransparency(schematic, 0f);
            } else {
                schematic.sprite = tile.sprite;
                SetTransparency(schematic, 1f);
            }
        }

        public void UpdateSelectedState(bool selected)
        {
            SetTransparency(frame, selected ? 1f : 0.6f);
            SetTransparency(schematic, selected ? 1f : 0.6f);
        }

        private void SetTransparency(Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

    }
}
