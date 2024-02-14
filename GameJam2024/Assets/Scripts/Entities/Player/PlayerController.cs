using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using GameLogic.player;
using GameLogic.world;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 500f;

    private PlayerInventory _playerInventory;

    public PlayerInventory PlayerInventory => _playerInventory;


    private Rigidbody2D _rigidbody;

    private Camera _cam;
    private World _world;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _cam = GameManager.Instance.Cam;
        _world = GameManager.Instance.World;
        TileRandomizer tileRandomizer = new TileRandomizer(GameManager.Instance.Tiles);
        _playerInventory = new PlayerInventory(tileRandomizer);
    }

    private void Update()
    {
        PlaceTileIfClicked();
        CheckForInvSlotChange();
    }

    private void PlaceTileIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mousePos);
            Vector2Int tilePos = new Vector2Int(Mathf.FloorToInt(mouseWorldPos.x), Mathf.FloorToInt(mouseWorldPos.y));

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

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 input = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _rigidbody.AddForce(input * (speed * Time.fixedDeltaTime), ForceMode2D.Force);
    }
}