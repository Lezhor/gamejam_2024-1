using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Tiles")]
    public TileData emptyTile;

    public TileData tile_we;
    public TileData tile_ns;
    public TileData tile_nwse;

    private TileData[][] tiles;

    private void Start()
    {
        tiles = new TileData[size.x][];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileData[size.y];
            for (int k = 0; k < tiles[i].Length; k++)
            {
                place(i, k, tile_nwse);
            }
        }
        
    }

    public void placeIfPossible(int xCell, int yCell, TileData tile)
    {
        if (canBePlaced(xCell, yCell, tile))
        {
            place(xCell, yCell, tile);
        }
    }

    private void place(int xCell, int yCell, TileData tile)
    {
        tiles[xCell][yCell] = tile;
        background.SetTile(new Vector3Int(xCell, yCell), tile.imageFloor);
        walls.SetTile(new Vector3Int(xCell, yCell), tile.imageWalls);
    }

    private bool canBePlaced(int xCell, int yCell, TileData tile)
    {
        if (!tile.mustConnect)
        {
            return false;
        }
        
        if (!tiles[xCell][yCell].destroyable)
        {
            return false;
        }

        if (inBounds(xCell, yCell + 1)
            && tiles[xCell][yCell + 1].mustConnect
            && tiles[xCell][yCell + 1].connectsBottom != tile.connectsTop)
        {
            return false;
        }
        if (inBounds(xCell + 1, yCell)
            && tiles[xCell + 1][yCell].mustConnect
            && tiles[xCell + 1][yCell].connectsLeft != tile.connectsRight)
        {
            return false;
        }
        
        if (inBounds(xCell, yCell - 1)
            && tiles[xCell][yCell - 1].mustConnect
            && tiles[xCell][yCell - 1].connectsTop != tile.connectsBottom)
        {
            return false;
        }
        if (inBounds(xCell - 1, yCell)
            && tiles[xCell - 1][yCell].mustConnect
            && tiles[xCell - 1][yCell].connectsRight != tile.connectsBottom)
        {
            return false;
        }

        return true;
    }

    private bool inBounds(int xCell, int yCell)
    {
        return xCell >= 0 && xCell < tiles.Length && yCell >= 0 && yCell < tiles[xCell].Length;
    }
    
}
