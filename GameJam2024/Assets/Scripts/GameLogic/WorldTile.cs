using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldTile
{

    public TileData data;
    private int x;
    private int y;
    private GameManager _gameManager;

    public WorldTile(TileData data, int x, int y, GameManager gameManager)
    {
        this.data = data;
        this.x = x;
        this.y = y;
        _gameManager = gameManager;
    }

}
