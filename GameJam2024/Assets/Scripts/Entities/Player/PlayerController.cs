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
    private PlayerInventory _playerInventory;

    public PlayerInventory PlayerInventory => _playerInventory;

    [Header("Player Settings")]
    [SerializeField]
    private float maxDigDistanceHorizontal = 1.5f;
    [SerializeField]
    private float maxDigDistanceVertical = 1f;

    private Camera _cam;
    private World _world;

    private void OnEnable()
    {
        _cam = GameManager.Instance.Cam;
        _world = GameManager.Instance.World;
        TileRandomizer tileRandomizer = new TileRandomizer(GameManager.Instance.Tiles);
        _playerInventory = new PlayerInventory(tileRandomizer);
    }

    protected override void FixedUpdateAddOn()
    {
    }

    private void Update()
    {
        CheckForWASDInput();
        PlaceTileIfClicked();
        CheckForInvSlotChange();
    }

    private void CheckForWASDInput()
    {
        MoveVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
                // TODO Call event!
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