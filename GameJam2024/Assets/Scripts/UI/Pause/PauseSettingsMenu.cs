using System;
using audio;
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

        private AudioManager _audioManager;
        
        private PauseMenuManager _pauseMenu;

        private void Start()
        {
            _pauseMenu = GameLogic.GameManager.Instance.UIManager.pauseMenu.GetComponent<PauseMenuManager>();
            _audioManager = GameLogic.GameManager.Instance.AudioManager;

            masterVolume.value = _audioManager.MasterVolume;
            musicVolume.value = _audioManager.MusicVolume;
            sfxVolume.value = _audioManager.SoundFXVolume;
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
            _audioManager.MasterVolume = value;
        }
        
        private void OnMusicVolumeChanged(float value)
        {
            _audioManager.MusicVolume = value;
        }
        
        private void OnSFXVolumeChanged(float value)
        {
            _audioManager.SoundFXVolume = value;
        }
        
        private void OnUIScaleChanged(float value)
        {
            Debug.Log("UI Scale changed: " + value);
        }
    }
}