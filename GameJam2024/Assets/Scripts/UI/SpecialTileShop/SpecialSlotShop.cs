using System;
using GameLogic;
using GameLogic.player;
using GameLogic.world;
using UnityEngine;

namespace UI.SpecialTileShop
{
    public class SpecialSlotShop : MonoBehaviour
    {
        public Button cancelButton;
        public Button buyButton;

        [SerializeField]
        private SpecialSlotShopItem[] items;

        private int _selected = -1;

        private TileData[] _tiles;

        private void OnEnable()
        {
            TileRegistry tileR = GameManager.Instance.Tiles;
            _tiles = new[]
            {
                tileR.nw,
                tileR.ws,
                tileR.se,
                tileR.ne,
                
                tileR.ns,
                tileR.nwse,
                tileR.we,
                
                tileR.nws,
                tileR.wse,
                tileR.nse,
                tileR.nwe,
            };
            Init();
            
            cancelButton.OnButtonClicked += OnCancelClicked;
            buyButton.OnButtonClicked += OnBuyClicked;
        }

        private void OnDisable()
        {
            cancelButton.OnButtonClicked -= OnCancelClicked;
            buyButton.OnButtonClicked -= OnBuyClicked;
        }

        public void Init()
        {
            buyButton.CanBeClicked = false;
            _selected = -1;
            InitAllItems();
        }

        private void InitAllItems()
        {
            
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Initialize(i, _tiles[i], this);
            }
        }

        public void ItemSelected(int index)
        {
            if (_selected == -1)
            {
                if (GameManager.Instance.PlayerScript.PlayerInventory.Gold >= 100)
                {
                    buyButton.CanBeClicked = true;
                }
            }
            else
            {
                items[_selected].UpdateSelectedState(false);
            }
            items[index].UpdateSelectedState(true);
            _selected = index;
        }

        private void OnCancelClicked()
        {
            GameManager.Instance.UIManager.HideSpecialSlotShop();
        }

        private void OnBuyClicked()
        {
            PlayerInventory inventory = GameManager.Instance.PlayerScript.PlayerInventory;
            if (inventory.Gold >= 100)
            {
                inventory.Gold -= 100;
                inventory.SetSpecialSlot(_tiles[_selected]);
                GameManager.Instance.AudioManager.Play("Shop Sound");
            }
            GameManager.Instance.UIManager.HideSpecialSlotShop();
        }
    }
}
