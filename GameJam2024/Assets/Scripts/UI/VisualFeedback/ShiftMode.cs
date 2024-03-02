using System;
using System.Collections.Generic;
using audio;
using GameLogic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using TileData = GameLogic.world.TileData;

namespace UI.VisualFeedback
{
    public class ShiftMode : MonoBehaviour
    {
        [Header("Tilemaps")] [SerializeField]
        private Tilemap scrollingFeedbackTilemap;
        [SerializeField] private float maxScrollingTilemapAlpha = .4f;
        [SerializeField] private Tilemap canBePlacedTilemap;
        [SerializeField] private float maxCanBePlacedTilemapAlpha = .2f;

        [Header("Fade")] [SerializeField] private float fadeTime = 1f;
        [Header("Other")] [SerializeField] private SpriteRenderer darkOverlay;
        [SerializeField] private float maxDarkOverlayAlpha = .3f;
        [Header("Audio")] 
        [Range(0, 22000)]
        [SerializeField] private int lowpassCutoffFreq = 5000;

        private float lowpassCutoffFreqLog;

        private float _lastPressTime;
        private float _currentValue;

        private bool _shiftPressed;

        private List<Vector2Int> _currentlyDisplayed = new();

        [SerializeField] private bool showPlaceHints = true;
        
        public bool ShowPlaceHints
        {
            get => showPlaceHints;
            set
            {
                if (showPlaceHints != value)
                {
                    showPlaceHints = value;
                    canBePlacedTilemap.gameObject.SetActive(showPlaceHints);
                }
            }
        }

        private void OnValidate()
        {
            lowpassCutoffFreqLog = Mathf.Log(lowpassCutoffFreq, 2);
            if (Application.isPlaying)
            {
                showPlaceHints = !showPlaceHints;
                ShowPlaceHints = !showPlaceHints;
            }
        }

        private GameManager _gameManager;
        private UIManager _uiManager;
        private AudioManager _audioManager;
        private PlayerController _player;

        private void Start()
        {
            lowpassCutoffFreqLog = Mathf.Log(lowpassCutoffFreq, 2);
            _gameManager = GameManager.Instance;
            _uiManager = _gameManager.UIManager;
            _audioManager = _gameManager.AudioManager;
            _player = _gameManager.PlayerScript;
            showPlaceHints = !showPlaceHints;
            ShowPlaceHints = !showPlaceHints;
            UpdateEverything(_currentValue);
            _gameManager.PlayerScript.PlayerInventory.OnActiveTileChanged += OnNewTileSelected;
            OnNewTileSelected(_gameManager.PlayerScript.PlayerInventory.CurrentSlot);
        }

        private void Update()
        {
            // TODO - Can be toggled
            if (_uiManager.GameActive && Input.GetKeyDown(KeyCode.LeftShift))
            {
                _lastPressTime = Time.time;
                _shiftPressed = true;
                _player.Crouch = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _lastPressTime = Time.time;
                _shiftPressed = false;
                _player.Crouch = false;
            }

            float lastFrameValue = _currentValue;

            float valueChange = (Time.time - _lastPressTime) * Time.deltaTime / fadeTime;
            _currentValue = Mathf.Clamp(_currentValue + (_shiftPressed ? valueChange : -valueChange), 0, 1);

            if (_currentValue != lastFrameValue)
            {
                UpdateEverything(_currentValue);
            }
        }

        private void UpdateEverything(float value)
        {
            _audioManager.MasterHighCutoffFreq = Mathf.Lerp(14.4252159f, lowpassCutoffFreqLog, value);
            SetTransparency(scrollingFeedbackTilemap, Mathf.Lerp(0, maxScrollingTilemapAlpha, value));
            SetTransparency(canBePlacedTilemap, Mathf.Lerp(0, maxCanBePlacedTilemapAlpha, value));
            SetTransparency(darkOverlay, Mathf.Lerp(0, maxDarkOverlayAlpha, value));
        }

        private void OnNewTileSelected(TileData tile)
        {
            
            foreach (Vector2Int pos in _currentlyDisplayed)
            {
                Vector3Int pos3D = new(pos.x, pos.y);
                canBePlacedTilemap.SetTile(pos3D, null);
            }

            if (tile == null)
            {
                _currentlyDisplayed = new();
            }
            else
            {
                _currentlyDisplayed = _gameManager.World.GetPlacesWhereTileCanBePlacedOn(tile);

                foreach (Vector2Int pos in _currentlyDisplayed)
                {
                    Vector3Int pos3D = new(pos.x, pos.y);
                    canBePlacedTilemap.SetTile(pos3D, tile.imageWalls);
                }
            }
        }

        private void SetTransparency(Tilemap tilemap, float alpha)
        {
            Color c = tilemap.color;
            c.a = alpha;
            tilemap.color = c;
        }
        
        private void SetTransparency(SpriteRenderer sprite, float alpha)
        {
            Color c = sprite.color;
            c.a = alpha;
            sprite.color = c;
        }
    }
}