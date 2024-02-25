using UnityEngine;

namespace UI.Pause
{
    public class PauseMenuManager : MonoBehaviour
    {
        public PauseMainMenu pauseMainMenu;

        public bool PauseMainMenuActive
        {
            get => pauseMainMenu.gameObject.activeSelf;
            private set => pauseMainMenu.gameObject.SetActive(value);
        }
        
        public PauseSettingsMenu pauseSettingsMenu;

        public bool PauseSettingsMenuActive
        {
            get => pauseSettingsMenu.gameObject.activeSelf;
            private set => pauseSettingsMenu.gameObject.SetActive(value);
        }

        private UIManager _uiManager;

        private void Start()
        {
            _uiManager = GameLogic.GameManager.Instance.UIManager;
        }

        private void OnEnable()
        {
            PauseMainMenuActive = true;
            PauseSettingsMenuActive = false;
        }

        private void OnDisable()
        {
            PauseMainMenuActive = false;
            PauseSettingsMenuActive = false;
        }

        public void OnResumeClicked()
        {
            _uiManager.HidePauseMenu();
        }

        public void OnBackClicked()
        {
            if (PauseSettingsMenuActive)
            {
                PauseSettingsMenuActive = false;
                PauseMainMenuActive = true;
            }
        }

        public void ShowSettingsMenu()
        {
            if (PauseMainMenuActive && !PauseSettingsMenuActive)
            {
                PauseMainMenuActive = false;
                PauseSettingsMenuActive = true;
            }
        }
    }
}