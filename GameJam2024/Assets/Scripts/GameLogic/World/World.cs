using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic.world
{
    public class World
    {
        private readonly GameManager _gameManager;
        private TileRegistry Registry => _gameManager.Tiles;
        private readonly WorldTile[][] _world;
        public readonly Vector2Int StartPos;

        public Vector2Int Size => new Vector2Int(_world.Length, _world.Length == 0 ? 0 : _world[0].Length);

        public World(Vector2Int size, Vector2Int startPos, GameManager gameManager)
        {
            _gameManager = gameManager;
            StartPos = startPos;

            _world = new WorldTile[size.x][];
            for (int i = 0; i < _world.Length; i++)
            {
                _world[i] = new WorldTile[size.y];
            }
            SetBackgroundAndFogAroundWorld(gameManager.worldBorderWidth);
            for (int i = 0; i < _world.Length; i++)
            {
                for (int k = 0; k < _world[i].Length; k++)
                {
                    Place(i, k, Registry.emptyTile);
                }
            }
            Place(startPos.x, startPos.y, Registry.nwse);
            ExploreTile(startPos.x, startPos.y);
        }

        public World(WorldTile[][] world, Vector2Int startPos, GameManager gameManager)
        {
            _world = world;
            StartPos = startPos;
            _gameManager = gameManager;
            SetBackgroundAndFogAroundWorld(gameManager.worldBorderWidth);
            ExploreTile(startPos.x, startPos.y);
        }

        private void SetBackgroundAndFogAroundWorld(int distance)
        {
            Vector3Int startPos = new Vector3Int(-distance, -distance);
            Vector3Int endPos = new Vector3Int(_world.Length + distance, _world[0].Length + distance);
            for (int y = startPos.y; y < endPos.y; y++)
            {
                for (int x = startPos.x; x < endPos.x; x++)
                {
                    if (!InBounds(x, y))
                    {
                        _gameManager.fog.SetTile(new Vector3Int(x, y), Registry.blockedTile.imageFogInvis);
                        _gameManager.background.SetTile(new Vector3Int(x, y), Registry.blockedTile.imageWalls);
                    }
                }
            }
        }

        private void ExploreTile(int x, int y)
        {
            if (InBounds(x, y))
            {
                // TODO - Explore neighbour tile if connected
                _world[x][y].SetExplored(true);
                foreach (WorldTile tile in GetConnectedNeighbours(x, y))
                {
                    if (!tile.IsExplored)
                    {
                        ExploreTile(tile.Pos.x, tile.Pos.y);
                    }
                }
                foreach (WorldTile tile in GetTilesInVisibleRadius(x, y))
                {
                    if (!tile.IsVisible)
                    {
                        tile.SetVisible(true);
                    }
                }
            }
        }

        private List<WorldTile> GetTilesInVisibleRadius(int x, int y)
        {
            List<WorldTile> list = new List<WorldTile>();
            
            for (int i = x - 2; i <= x + 2; i++)
            {
                for (int k = y - 2; k <= y + 2; k++)
                {
                    if (InBounds(i, k)
                        && (i != x || k != y)
                        && Math.Abs(i - x) + Math.Abs(k - y) < 4)
                    {
                        list.Add(_world[i][k]);
                    }
                }
            }
            
            return list;
        }

        private List<WorldTile> GetNeighbours(int x, int y)
        {

            return new List<Vector2Int>(new[]
                {
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0),
                })
                .Select(v => v + new Vector2Int(x, y))
                .Where(v => InBounds(v.x, v.y))
                .Select(v => _world[v.x][v.y])
                .ToList();
        }

        private List<WorldTile> GetConnectedNeighbours(int x, int y)
        {
            if (!InBounds(x, y)) return new List<WorldTile>();

            WorldTile tile = _world[x][y];
            List<WorldTile> list = new List<WorldTile>();
            
            if (InBounds(x, y + 1)
                && tile.Data.connectsTop
                && _world[x][y + 1].Data.connectsBottom)
            {
                list.Add(_world[x][y + 1]);
            }
            if (InBounds(x + 1, y)
                && tile.Data.connectsRight
                && _world[x + 1][y].Data.connectsLeft)
            {
                list.Add(_world[x + 1][y]);
            }
            if (InBounds(x, y - 1)
                && tile.Data.connectsBottom
                && _world[x][y - 1].Data.connectsTop)
            {
                list.Add(_world[x][y - 1]);
            }
            if (InBounds(x - 1, y)
                && tile.Data.connectsLeft
                && _world[x - 1][y].Data.connectsRight)
            {
                list.Add(_world[x - 1][y]);
            }

            return list;
        }

        /**
         * Checks if one of the connected neighbours is explored - If true and self is not explored it Sets itself to explored!
         */
        private void UpdateTileState(int x, int y)
        {
            bool shouldBeVisible = false;
            foreach (WorldTile tile in GetTilesInVisibleRadius(x, y))
            {
                if (tile.IsExplored)
                {
                    shouldBeVisible = true;
                    break;
                }
            }

            if (shouldBeVisible && !_world[x][y].IsVisible)
            {
                _world[x][y].SetVisible(true);
            }
            
            bool shouldBeExplored = false;
            foreach (WorldTile tile in GetConnectedNeighbours(x, y))
            {
                if (tile.IsExplored)
                {
                    shouldBeExplored = true;
                    break;
                }
            }

            if (shouldBeExplored && !_world[x][y].IsExplored)
            {
                ExploreTile(x, y);
            }
        }

        public void RedrawAllTiles()
        {
            foreach (WorldTile[] row in _world)
            {
                foreach (WorldTile tile in row)
                {
                    tile.redrawOnTilemaps();
                }
            }
        }


        /**
         * Returns true if placed successfully
         */
        public bool PlaceIfPossible(int xCell, int yCell, TileData tile)
        {
            if (!CanBePlaced(xCell, yCell, tile))
            {
                return false;
            }
            Place(xCell, yCell, tile);
            UpdateTileState(xCell, yCell);
            return true;
        }

        private void Place(int x, int y, TileData tile)
        {
            _world[x][y] = new WorldTile(tile, x, y, _gameManager);
        }

        private bool CanBePlaced(int xCell, int yCell, TileData tile)
        {
            if (!tile.mustConnect)
            {
                return false;
            }

            if (!InBounds(xCell, yCell))
            {
                return false;
            }

            if (!_world[xCell][yCell].Data.diggable)
            {
                return false;
            }

            if (!InBounds(xCell, yCell + 1) && tile.connectsTop
                || !InBounds(xCell + 1, yCell) && tile.connectsRight
                || !InBounds(xCell, yCell - 1) && tile.connectsBottom
                || !InBounds(xCell - 1, yCell) && tile.connectsLeft
                )
            {
                return false;
            }

            if (!GetNeighbours(xCell, yCell).Any(tile => tile.IsExplored))
            {
                return false;
            }

            if (InBounds(xCell, yCell + 1)
                && _world[xCell][yCell + 1].Data.mustConnect
                && _world[xCell][yCell + 1].Data.connectsBottom != tile.connectsTop)
            {
                return false;
            }

            if (InBounds(xCell + 1, yCell)
                && _world[xCell + 1][yCell].Data.mustConnect
                && _world[xCell + 1][yCell].Data.connectsLeft != tile.connectsRight)
            {
                return false;
            }

            if (InBounds(xCell, yCell - 1)
                && _world[xCell][yCell - 1].Data.mustConnect
                && _world[xCell][yCell - 1].Data.connectsTop != tile.connectsBottom)
            {
                return false;
            }

            if (InBounds(xCell - 1, yCell)
                && _world[xCell - 1][yCell].Data.mustConnect
                && _world[xCell - 1][yCell].Data.connectsRight != tile.connectsLeft)
            {
                return false;
            }

            return true;
        }

        private bool InBounds(int xCell, int yCell)
        {
            return xCell >= 0 && xCell < _world.Length && yCell >= 0 && yCell < _world[xCell].Length;
        }
    }
}