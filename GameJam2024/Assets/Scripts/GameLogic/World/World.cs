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
        private Vector2Int _startPos;

        public World(Vector2Int size, Vector2Int startPos, GameManager gameManager)
        {
            _gameManager = gameManager;

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
            Place(startPos.x - 1, startPos.y, Registry.ne);
            Place(startPos.x + 2, startPos.y + 1, Registry.se);
            Place(startPos.x, startPos.y - 2, Registry.we);
            Place(startPos.x - 1, startPos.y + 2, Registry.ns);
            Place(startPos.x - 1, startPos.y + 3, Registry.se);
            Place(startPos.x, startPos.y + 3, Registry.we);
            ExploreTile(startPos.x, startPos.y);
        }

        private void ExploreTile(int x, int y)
        {
            if (InBounds(x, y))
            {
                // TODO - Explore neighbour tile if connected
                _world[x][y].SetExplored(true);
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


        /**
         * Returns true if placed successfully
         */
        public bool PlaceIfPossible(int xCell, int yCell, TileData tile)
        {
            if (!CanBePlaced(xCell, yCell, tile))
                return false;
            Place(xCell, yCell, tile);
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

            if (!_world[xCell][yCell].Data.destroyable)
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
                && _world[xCell - 1][yCell].Data.connectsRight != tile.connectsBottom)
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