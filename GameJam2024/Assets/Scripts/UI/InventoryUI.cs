using System;
using GameLogic;
using GameLogic.player;
using GameLogic.world;
using UnityEngine;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        public InventorySlotUI slot1;
        public InventorySlotUI slot2;
        public InventorySlotUI slot3;
        public InventorySlotUI slot4;

        private InventorySlotUI GetSlot(int index) => index switch
        {
            0 => slot1,
            1 => slot2,
            2 => slot3,
            3 => slot4,
            _ => null
        };

        private PlayerInventory _playerInventory;

        private UIManager _uiManager;

        private void Awake()
        {
            _uiManager = GameManager.Instance.UIManager;
            
            slot1.SetLabel(1);
            slot2.SetLabel(2);
            slot3.SetLabel(3);
            slot4.SetLabel(4);
        }

        private void Start()
        {
            _playerInventory = GameManager.Instance.PlayerScript.PlayerInventory;
            
            _playerInventory.OnSlotContentChanged += UpdateSlotContent;
            _playerInventory.OnActiveSlotChanged += UpdateSelectedSlot;
            Init();
        }

        private void OnEnable()
        {
            slot1.OnClicked += OnSlot1Clicked;
            slot2.OnClicked += OnSlot2Clicked;
            slot3.OnClicked += OnSlot3Clicked;
            slot4.OnClicked += OnSlot4Clicked;
        }

        private void Init()
        {
            slot1.UpdateSchematic(_playerInventory.Slot(0));
            slot2.UpdateSchematic(_playerInventory.Slot(1));
            slot3.UpdateSchematic(_playerInventory.Slot(2));
            slot4.UpdateSchematic(_playerInventory.Slot(3));
            UpdateSelectedSlot(_playerInventory.CurrentSlotIndex);
        }

        private void OnDisable()
        {
            _playerInventory.OnSlotContentChanged -= UpdateSlotContent;
            _playerInventory.OnActiveSlotChanged -= UpdateSelectedSlot;
            
            slot1.OnClicked -= OnSlot1Clicked;
            slot2.OnClicked -= OnSlot2Clicked;
            slot3.OnClicked -= OnSlot3Clicked;
            slot4.OnClicked -= OnSlot4Clicked;
        }

        private void UpdateSlotContent(int index, TileData tile)
        {
            GetSlot(index).UpdateSchematic(tile);
        }

        private void UpdateSelectedSlot(int newSelectedSlot)
        {
            slot1.UpdateSelectedState(newSelectedSlot == 0);
            slot2.UpdateSelectedState(newSelectedSlot == 1);
            slot3.UpdateSelectedState(newSelectedSlot == 2);
            slot4.UpdateSelectedState(newSelectedSlot == 3);
        }

        private void OnSlot1Clicked() => OnSlotClicked(0);
        private void OnSlot2Clicked() => OnSlotClicked(1);
        private void OnSlot3Clicked() => OnSlotClicked(2);
        private void OnSlot4Clicked() => OnSlotClicked(3);

        private void OnSlotClicked(int index)
        {
            if (_uiManager.GameActive)
            {
                _playerInventory.CurrentSlotIndex = index;
            }
        }
    }
}