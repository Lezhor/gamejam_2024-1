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

    [SerializeField]
    private float speed = 500f;

    [SerializeField] 
    private TileData currentTile;

    public TileData CurrentTile
    {
        get
        {
            return currentTile;
        }

        set
        {
            currentTile = value;
            // TODO - Call Event to update UI
        }
    }

    private TileRandomizer _tileRandomizer;

    private Rigidbody2D _rigidbody;

    private Camera _cam;
    private World _world;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _cam = GameManager.Instance.Cam;
        _world = GameManager.Instance.World;
        _tileRandomizer = new TileRandomizer(GameManager.Instance.Tiles);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceActiveTileOnCursorField();
        }
    }

    private void PlaceActiveTileOnCursorField()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mousePos);
        Vector2Int tilePos = new Vector2Int(Mathf.FloorToInt(mouseWorldPos.x), Mathf.FloorToInt(mouseWorldPos.y));

        if (_world.PlaceIfPossible(tilePos.x, tilePos.y, CurrentTile))
        {
            CurrentTile = _tileRandomizer.GetRandomTile();
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
