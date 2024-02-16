using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using GameLogic;
using GameLogic.player;
using GameLogic.world;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : EntityMovement
{
    
    public event Action<Vector2Int, Vector2Int> OnMovedToNewTile;
    public event Action<Vector2Int> OnActionKeyPressed;

    private Vector2Int _tilePosLastFrame;
    
    
    private PlayerInventory _playerInventory;

    public PlayerInventory PlayerInventory => _playerInventory;

    [Header("Player Settings")]
    [SerializeField]
    private float maxDigDistanceHorizontal = 1.5f;
    [SerializeField]
    private float maxDigDistanceVertical = 1f;

    private Camera _cam;
    private World _world;

    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _cam = _gameManager.Cam;
        _world = _gameManager.World;
        _tilePosLastFrame = GetTilePos(transform.position);
        TileRandomizer tileRandomizer = new TileRandomizer(_gameManager.Tiles);
        _playerInventory = new PlayerInventory(tileRandomizer);
    }

    protected override void FixedUpdateAddOn()
    {
    }

    private void Update()
    {
        CheckForWASDInput();
        PlaceTileIfClicked();
        CheckIfMovedToNewTile();
        CheckIfActionKeyPressed();
        CheckForInvSlotChange();
    }

    private void CheckForWASDInput()
    {
        MoveVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void CheckIfMovedToNewTile()
    {
        Vector2Int tilePos = GetTilePos(transform.position);
        if (!tilePos.Equals(_tilePosLastFrame))
        {
            OnMovedToNewTile?.Invoke(_tilePosLastFrame, tilePos);
            _tilePosLastFrame = tilePos;
        }
    }

    private void CheckIfActionKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnActionKeyPressed?.Invoke(GetTilePos(transform.position));
        }
    }

    private Vector2Int GetTilePos(Vector3 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }

    private void PlaceTileIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mousePos);
            Vector2Int tilePos = new Vector2Int(Mathf.FloorToInt(mouseWorldPos.x), Mathf.FloorToInt(mouseWorldPos.y));

            Vector3 tileWorldPos = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f);

            Vector3 distanceToTile = tileWorldPos - transform.position;

            distanceToTile = new Vector3(distanceToTile.x / maxDigDistanceHorizontal,
                distanceToTile.y / maxDigDistanceVertical);
            

            if (distanceToTile.magnitude > 1)
            {
                _gameManager.PlaceEvents?.InvokeTriedToPlaceTileToFar(tilePos);
                return;
            }

            if (_world.PlaceIfPossible(tilePos.x, tilePos.y, _playerInventory.CurrentSlot))
            {
                _playerInventory.ReplaceCurrentSlot();
            }
        }
    }

    private void CheckForInvSlotChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerInventory.CurrentSlotIndex = 0;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerInventory.CurrentSlotIndex = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _playerInventory.CurrentSlotIndex = 2;
        } else if (_playerInventory.Slot(3) != null && Input.GetKeyDown(KeyCode.Alpha4))
        {
            _playerInventory.CurrentSlotIndex = 3;
        }
        else
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta > 0)
            {
                _playerInventory.DecrementSlotIndexIfPossible();
            } else if (scrollDelta < 0)
            {
                _playerInventory.IncrementSlotIndexIfPossible();
            }
        }
    }

}