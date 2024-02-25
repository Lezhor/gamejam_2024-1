using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pause
{
    public class PauseSettingsMenu : MonoBehaviour
    {
        
        public Slider masterVolume;
        public Slider musicVolume;
        public Slider sfxVolume;
        public Slider uiScale;

        public Button backButton;
        
        private PauseMenuManager _pauseMenu;

        private void Start()
        {
            _pauseMenu = GameLogic.GameManager.Instance.UIManager.pauseMenu.GetComponent<PauseMenuManager>();
        }

        public void OnEnable()
        {
            masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolume.onValueChanged.AddListener(OnSFXVolumeChanged);
            uiScale.onValueChanged.AddListener(OnUIScaleChanged);

            backButton.OnButtonClicked += OnBackPressed;
        }

        private void OnDisable()
        {
            masterVolume.onValueChanged.RemoveAllListeners();
            musicVolume.onValueChanged.RemoveAllListeners();
            sfxVolume.onValueChanged.RemoveAllListeners();
            uiScale.onValueChanged.RemoveAllListeners();

            backButton.OnButtonClicked -= OnBackPressed;
        }

        private void OnBackPressed()
        {
            _pauseMenu.OnBackClicked();
        }

        private void OnMasterVolumeChanged(float value)
        {
            Debug.Log("Master Volume changed: " + value);
        }
        
        private void OnMusicVolumeChanged(float value)
        {
            Debug.Log("Music Volume changed: " + value);
        }
        
        private void OnSFXVolumeChanged(float value)
        {
            Debug.Log("SFX Volume changed: " + value);
        }
        
        private void OnUIScaleChanged(float value)
        {
            Debug.Log("UI Scale changed: " + value);
        }
    }
}