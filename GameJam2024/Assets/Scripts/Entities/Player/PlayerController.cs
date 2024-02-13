using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using GameLogic.world;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 500f;

    [SerializeField] 
    private TileData currentTile;

    private Rigidbody2D _rigidbody;

    private Camera _cam;
    private World _world;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _cam = GameManager.Instance.Cam;
        _world = GameManager.Instance.World;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceActiveTileOnCursorField();
        }
    }

    private bool PlaceActiveTileOnCursorField()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mousePos);
        Vector2Int tilePos = new Vector2Int(Mathf.FloorToInt(mouseWorldPos.x), Mathf.FloorToInt(mouseWorldPos.y));

        return _world.PlaceIfPossible(tilePos.x, tilePos.y, currentTile);
        
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
