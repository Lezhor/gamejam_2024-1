using System;
using System.Collections;
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

        public bool SpecialSlotShopActive
        {
            get => specialSlotShop.activeSelf;
            private set => specialSlotShop.SetActive(value);
        }

        public GameObject winScreen;

        public bool WinScreenActive
        {
            get => winScreen.activeSelf;
            private set => winScreen.SetActive(value);
        }
        
        public GameObject gameOverScreen;

        public bool GameOverScreenActive
        {
            get => gameOverScreen.activeSelf;
            private set => gameOverScreen.SetActive(value);
        }

        public GameObject pauseMenu;

        public bool PauseMenuActive
        {
            get => pauseMenu.activeSelf;
            private set => pauseMenu.SetActive(value);
        }

        private PlayerController _player;

        public bool GameActive => !SpecialSlotShopActive && !PauseMenuActive && !WinScreenActive && !GameOverScreenActive;

        private void Start()
        {
            _player = GameManager.Instance.PlayerScript;
            _specialSlotShop = specialSlotShop.GetComponent<SpecialSlotShop>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SpecialSlotShopActive)
                {
                    HideSpecialSlotShop();
                } else if (PauseMenuActive)
                {
                    HidePauseMenu();
                }
                else
                {
                    ShowPauseMenu();
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
            if (!SpecialSlotShopActive)
            {
                SpecialSlotShopActive = true;
                _specialSlotShop.Init();
                _player.DisableInput(true, true, true, true, true);
            }
        }

        public void HideSpecialSlotShop()
        {
            if (SpecialSlotShopActive)
            {
                SpecialSlotShopActive = false;
                _player.EnableInput(true, true, true, true, true);
            }
        }

        public void ShowPauseMenu()
        {
            if (!PauseMenuActive)
            {
                PauseMenuActive = true;
                _player.DisableInput(true, true, true, true, true);
            }
        }

        public void HidePauseMenu()
        {
            if (PauseMenuActive)
            {
                PauseMenuActive = false;
                _player.EnableInput(true, true, true, true, true);
            }
        }

        public void OnPauseButtonClicked()
        {
            if (PauseMenuActive)
            {
                HidePauseMenu();
            } else if (!PauseMenuActive && GameActive)
            {
                ShowPauseMenu();
            }
        }

        public void ShowWinScreen()
        {
            if (!WinScreenActive)
            {
                WinScreenActive = true;
                _player.DisableInput(true, true, true, true, true);
            }
        }
        public void ShowGameOverScreen()
        {
            if (!GameOverScreenActive)
            {
                GameManager.Instance.AudioManager.Play("Game Over");
                DoAfterDelay(() =>
                {
                    GameOverScreenActive = true;
                    _player.DisableInput(true, true, true, true, true);
                }, .4f);
            }
        }

        private void DoAfterDelay(Action action, float delay)
        {
            StartCoroutine(DoAfterDelayIEnumerator(action, delay));
        }

        private IEnumerator DoAfterDelayIEnumerator(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}