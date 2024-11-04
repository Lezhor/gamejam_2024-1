using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Pause
{
    public class PauseMainMenu : MonoBehaviour
    {
        public Button resumeButton;
        public Button retryButton;
        public Button settingsButton;
        public Button mainMenuButton;

        private PauseMenuManager _pauseMenu;

        private void Start()
        {
            _pauseMenu = GameLogic.GameManager.Instance.UIManager.pauseMenu.GetComponent<PauseMenuManager>();
        }

        private void OnEnable()
        {
            resumeButton.OnButtonClicked += OnResumeButtonClicked;
            retryButton.OnButtonClicked += OnRetryButtonClicked;
            settingsButton.OnButtonClicked += OnSettingsButtonClicked;
            mainMenuButton.OnButtonClicked += OnMainMenuButtonClicked;
        }

        private void OnDisable()
        {
            resumeButton.OnButtonClicked -= OnResumeButtonClicked;
            retryButton.OnButtonClicked -= OnRetryButtonClicked;
            settingsButton.OnButtonClicked -= OnSettingsButtonClicked;
            mainMenuButton.OnButtonClicked -= OnMainMenuButtonClicked;
        }

        public void OnResumeButtonClicked()
        {
            _pauseMenu.OnResumeClicked();
        }
        
        public void OnRetryButtonClicked()
        {
            // TODO
            Debug.Log("Restart");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void OnSettingsButtonClicked()
        {
            _pauseMenu.ShowSettingsMenu();
        }
        
        public void OnMainMenuButtonClicked()
        {
            // TODO
            Debug.Log("Going to Main Menu");
        }

    }
}