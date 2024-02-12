using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class World
    {
        private readonly GameManager _gameManager;
        private TileRegistry Registry => _gameManager.Tiles;
        private readonly WorldTile[][] _world;
        public readonly Vector2Int StartPos;

        public World(Vector2Int size, Vector2Int startPos, GameManager gameManager)
        {
            _gameManager = gameManager;
            StartPos = startPos;

            _world = new WorldTile[size.x][];
            for (int i = 0; i < _world.Length; i++)
            {
                _world[i] = new WorldTile[size.y];
                for (int k = 0; k < _world[i].Length; k++)
                {
                    Place(i, k, Registry.emptyTile);
                }
            }
            
            Place(startPos.x, startPos.y, Registry.nwse);
            Place(startPos.x + 1, startPos.y, Registry.we);
            Place(startPos.x - 1, startPos.y, Registry.nse);
            Place(startPos.x + 2, startPos.y + 1, Registry.se);
            Place(startPos.x + 3, startPos.y + 1, Registry.nws);
            Place(startPos.x, startPos.y - 2, Registry.we);
            Place(startPos.x - 1, startPos.y + 2, Registry.ns);
            Place(startPos.x - 1, startPos.y + 1, Registry.nse);
            Place(startPos.x - 1, startPos.y + 3, Registry.se);
            Place(startPos.x, startPos.y + 3, Registry.nwe);
            ExploreTile(startPos.x, startPos.y);
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
                _world[x][y].SetExplored(true);
            }
        }


        /**
         * Returns true if placed successfully
         */
        public bool PlaceIfPossible(int xCell, int yCell, TileData tile)
        {
            Debug.Log("Trying to place: " + tile + " at position ( " + xCell + " | " + yCell + " )");
            if (!CanBePlaced(xCell, yCell, tile))
            {
                Debug.Log("Cannot be placed");
                return false;
            }

            Debug.Log("Can be placed");
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

            if (!_world[xCell][yCell].Data.diggable)
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