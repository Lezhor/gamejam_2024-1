using System;
using GameLogic;
using UI.SpecialTileShop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIManager : MonoBehaviour
    {

        public GameObject specialSlotShop;
        private SpecialSlotShop _specialSlotShop;
        private bool _specialSlotShopActive = false;

        private PlayerController _player;

        public bool GameActive => !_specialSlotShopActive;

        private void Start()
        {
            _player = GameManager.Instance.PlayerScript;
            _specialSlotShop = specialSlotShop.GetComponent<SpecialSlotShop>();
            specialSlotShop.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_specialSlotShopActive)
                {
                    HideSpecialSlotShop();
                }
            }
        }

        public bool IsMouseOverUI()
        {
            // TODO - With raycasts
            return EventSystem.current.IsPointerOverGameObject();
        }

        public void ShowSpecialSlotShop()
        {
            if (!_specialSlotShopActive)
            {
                _specialSlotShopActive = true;
                specialSlotShop.SetActive(true);
                _specialSlotShop.Init();
                _player.DisableInput(true, true, true, true, true);
            }
        }

        public void HideSpecialSlotShop()
        {
            if (_specialSlotShopActive)
            {
                _specialSlotShopActive = false;
                specialSlotShop.SetActive(false);
                _player.EnableInput(true, true, true, true, true);
            }
        }

    }
}
