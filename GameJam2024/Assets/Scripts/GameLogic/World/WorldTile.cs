using System.Collections;
using System.Collections.Generic;
using GameLogic;
using GameLogic.world.tiles;
using GameLogic.world.tiles.actions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = GameLogic.world.TileData;

/**
 * The data for the tile (images, connections, mustConnect) is stored in the _tileData.
 * However params vary from tile to tile are saved here: _x, _y, _visible, _dark
 */
public class WorldTile
{
    private TileData _tileData;
    public TileData Data => _tileData;

    private ActionTile _actionTile;

    private TileAction TileAction { get; }

    public ActionTile.Variant ActionTileVariant { get; }

    private int _x;
    private int _y;

    private bool _visible;
    public bool IsVisible => _visible;
    private bool _explored;
    public bool IsExplored => _explored;

    private GameManager _gameManager;

    public Vector3Int Pos => new(_x, _y);

    public WorldTile(TileData tileData, int x, int y, GameManager gameManager) : this(tileData, null, x, y, gameManager)
    {
    }
    
    public WorldTile(TileData tileData, ActionTile actionTile, int x, int y, GameManager gameManager) : this(tileData, actionTile, x, y, gameManager,
        false, false)
    {
    }

    public WorldTile(TileData tileData, ActionTile actionTile, int x, int y, GameManager gameManager, bool visible, bool explored)
    {
        _tileData = tileData;
        _actionTile = actionTile;
        if (actionTile != null)
        {
            List<ActionTile.Variant> fittingVariants = _actionTile.GetFittingVariants(_tileData);
            ActionTileVariant = fittingVariants.Count != 0 ? fittingVariants[Random.Range(0, fittingVariants.Count)] : null;

            TileAction = actionTile.possibleActions.Count != 0 ? actionTile.possibleActions[Random.Range(0, actionTile.possibleActions.Count)] : null;
        }
        _x = x;
        _y = y;
        _gameManager = gameManager;
        _visible = visible;
        _explored = explored;
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

    public void SetExplored(bool explored)
    {
        if (_explored != explored)
        {
            _explored = explored;
            if (_explored && !_visible)
            {
                _visible = true;
            }

            redrawOnTilemaps();
        }
    }

    private void redrawOnTilemaps()
    {
        if (_visible || _gameManager.SpectatorMode)
        {
            _gameManager.fog.SetTile(Pos, null);
            if (_explored || _gameManager.SpectatorMode)
            {
                _gameManager.background.SetTile(Pos, _tileData.imageFloor);
                _gameManager.walls.SetTile(Pos, _tileData.imageWalls);
                _gameManager.fogPath.SetTile(Pos, null);
                TileBase foregroundTile = ActionTileVariant?.GetTile(TileAction != null && TileAction.Executed);
                _gameManager.foreground.SetTile(Pos, foregroundTile);
            }
            else
            {
                _gameManager.background.SetTile(Pos, _tileData.imageFloorDark);
                _gameManager.walls.SetTile(Pos, _tileData.imageWallsDark);
                _gameManager.fogPath.SetTile(Pos, _tileData.imageFogPath);
                _gameManager.foreground.SetTile(Pos, null);
            }
        }
        else
        {
            _gameManager.background.SetTile(Pos, _tileData.imageInvis);
            _gameManager.fog.SetTile(Pos, _tileData.imageFogInvis);
            _gameManager.walls.SetTile(Pos, null);
            _gameManager.foreground.SetTile(Pos, null);
        }
    }
}