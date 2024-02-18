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
        
        protected override void Redraw()
        {
            base.Redraw();
            if (schematicImage.sprite == nullSchematicSprite)
            {
                foreground.sprite = foregroundSprite;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on Special Slot!!!");
        }
    }
}