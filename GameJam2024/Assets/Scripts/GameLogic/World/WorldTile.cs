using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * The data for the tile (images, connections, mustConnect) is stored in the _tileData.
 * However params vary from tile to tile are saved here: _x, _y, _visible, _dark
 */
public class WorldTile
{

    private TileData _tileData;
    public TileData Data => _tileData;
    
    // TODO add _tileContent (What's on the tile)
    
    private int _x;
    private int _y;

    private bool _visible;
    private bool _dark;
    
    private GameManager _gameManager;

    private Vector3Int Pos => new(_x, _y);

    public WorldTile(TileData tileData, int x, int y, GameManager gameManager) : this(tileData, x, y, gameManager, false, true)
    {
    }

    public WorldTile(TileData tileData, int x, int y, GameManager gameManager, bool visible, bool dark)
    {
        _tileData = tileData;
        _x = x;
        _y = y;
        _gameManager = gameManager;
        _visible = visible;
        _dark = dark;
        redrawOnTilemaps();
    }

    public void SetVisible(bool visible)
    {
        if (_visible != visible)
        {
            _visible = visible;
            redrawOnTilemaps();
        }
    }

    public void SetDark(bool dark)
    {
        if (_dark != dark)
        {
            _dark = dark;
            redrawOnTilemaps();
        }
    }

    private void redrawOnTilemaps()
    {
        if (_visible)
        {
            _gameManager.background.SetTile(Pos, _dark ? _tileData.imageFloorDark : _tileData.imageFloor);
            _gameManager.walls.SetTile(Pos, _dark ? _tileData.imageWallsDark : _tileData.imageWalls);
        }
        else
        {
            _gameManager.background.SetTile(Pos, _tileData.imageInvis);
            _gameManager.walls.SetTile(Pos, null);
        }
    }

}
