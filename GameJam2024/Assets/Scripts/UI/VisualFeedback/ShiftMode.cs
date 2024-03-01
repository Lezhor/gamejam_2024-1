using System.Collections.Generic;
using GameLogic;
using UnityEngine;
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

        private float _lastPressTime;
        private float _currentValue;

        private bool _shiftPressed;

        private List<Vector2Int> _currentlyDisplayed = new();

        private GameManager _gameManager;
        private UIManager _uiManager;
        private PlayerController _player;

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _uiManager = _gameManager.UIManager;
            _player = _gameManager.PlayerScript;
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
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _lastPressTime = Time.time;
                _shiftPressed = false;
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