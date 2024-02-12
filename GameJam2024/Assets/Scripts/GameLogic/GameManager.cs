using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Settings")] 
    public Vector2Int size = new(9, 7);
    [Header("Tilemaps")]
    public Tilemap background;
    public Tilemap walls;
    public Tilemap foreground;

    [Header("Resources")] 
    [SerializeField]
    private TileRegistry _tiles;

    public TileRegistry Tiles => _tiles;

    private World _world;

    private void Start()
    {
        // TODO - WorldGenerator should create World
        _world = new World(size, this);
    }

}
