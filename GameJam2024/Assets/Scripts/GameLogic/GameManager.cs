using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Settings")] public Vector2Int size = new(9, 7);

    [Header("Other")]
    [SerializeField]
    private GameObject _player;
    public GameObject Player => _player;
    [SerializeField]
    private Camera _camera;
    public Camera Cam => _camera;
    
    [Header("Tilemaps")] public Tilemap background;
    public Tilemap walls;
    public Tilemap foreground;

    [Header("Resources")] [SerializeField] private TileRegistry _tiles;
    

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public TileRegistry Tiles => _tiles;

    private World _world;
    public World World => _world;

    private void Awake()
    {
        _instance = this;
        
        // TODO - WorldGenerator should create World
        Vector2Int startPos = new(1, Mathf.CeilToInt(size.y / 2f));
        _world = new World(size, startPos, this);
    }

    private void Start()
    {
        _player.transform.position = new Vector3(_world.StartPos.x + 0.5f, _world.StartPos.y + 0.5f);
    }
}