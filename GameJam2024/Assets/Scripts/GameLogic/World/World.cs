using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.world.generators;
using UnityEngine;

namespace GameLogic.world
{
    public class World
    {
        private readonly GameManager _gameManager;
        private readonly PlayerController _player;
        private TileRegistry Registry => _gameManager.Tiles;
        private readonly WorldTile[][] _world;
        public readonly Vector2Int StartPos;
        public readonly Vector2Int[] EndPos;
        

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

        public World(WorldTile[][] world, Vector2Int startPos, Vector2Int[] endPos, GameManager gameManager)
        {
            _world = world;
            StartPos = startPos;
            EndPos = endPos;
            _gameManager = gameManager;
            _player = _gameManager.PlayerScript;
            SetBackgroundAndFogAroundWorld(gameManager.worldBorderWidth);
            ExploreTile(startPos.x, startPos.y);
            foreach (Vector2Int pos in endPos)
            {
                ExploreTile(pos.x, pos.y);
                _gameManager.GoalManager.InstantiateForTile(_world[pos.x][pos.y]);
            }
            _player.OnMovedToNewTile += OnPlayerMovedToNewTile;
            _player.OnActionKeyPressed += OnPlayerPressedActionKey;
        }

        private void OnPlayerMovedToNewTile(Vector2Int from, Vector2Int to)
        {
            _world[from.x][from.y].TileAction?.OnPlayerExitTile(_player);
            _world[to.x][to.y].TileAction?.OnPlayerEnterTile(_player);
        }

        private void OnPlayerPressedActionKey(Vector2Int position)
        {
            _world[position.x][position.y].TileAction?.Invoke(_player);
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
                    tile.RedrawOnTilemaps();
                }
            }
        }


        /**
         * Returns true if placed successfully
         */
        public bool PlaceIfPossible(int xCell, int yCell, TileData tile)
        {
            if (!CanBePlaced(xCell, yCell, tile, new()))
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

        public bool CanBePlaced(int x, int y, TileData tile, List<Vector2Int> reasonsWhyCantBePlaced)
        {
            bool canBePlaced = true;
            
            if (!tile.mustConnect)
            {
                return false;
            }

            if (!InBounds(x, y))
            {
                return false;
            }

            if (!_world[x][y].Data.diggable)
            {
                return false;
            }

            if (!GetNeighbours(x, y).Any(worldTile => worldTile.IsExplored && Connected(worldTile, x, y, tile)))
            {
                return false;
            }

            if (!InBounds(x, y + 1) && tile.connectsTop)
            {
                reasonsWhyCantBePlaced.Add(new Vector2Int(x, y + 1));
                canBePlaced = false;
            }

            if (!InBounds(x + 1, y) && tile.connectsRight)
            {
                reasonsWhyCantBePlaced.Add(new Vector2Int(x + 1, y));
                canBePlaced = false;
            }

            if (!InBounds(x, y - 1) && tile.connectsBottom)
            {
                reasonsWhyCantBePlaced.Add(new Vector2Int(x, y - 1));
                canBePlaced = false;
            }

            if (!InBounds(x - 1, y) && tile.connectsLeft)
            {
                reasonsWhyCantBePlaced.Add(new Vector2Int(x - 1, y));
                canBePlaced = false;
            }

            if (InBounds(x, y + 1)
                && _world[x][y + 1].Data.mustConnect
                && _world[x][y + 1].Data.connectsBottom != tile.connectsTop)
            {
                reasonsWhyCantBePlaced.Add(new Vector2Int(x, y + 1));
                canBePlaced = false;
            }

            if (InBounds(x + 1, y)
                && _world[x + 1][y].Data.mustConnect
                && _world[x + 1][y].Data.connectsLeft != tile.connectsRight)
            {
                reasonsWhyCantBePlaced.Add(new Vector2Int(x + 1, y));
                canBePlaced = false;
            }

            if (InBounds(x, y - 1)
                && _world[x][y - 1].Data.mustConnect
                && _world[x][y - 1].Data.connectsTop != tile.connectsBottom)
            {
                reasonsWhyCantBePlaced.Add(new Vector2Int(x, y - 1));
                canBePlaced = false;
            }

            if (InBounds(x - 1, y)
                && _world[x - 1][y].Data.mustConnect
                && _world[x - 1][y].Data.connectsRight != tile.connectsLeft)
            {
                reasonsWhyCantBePlaced.Add(new Vector2Int(x - 1, y));
                canBePlaced = false;
            }

            return canBePlaced;
        }

        private bool Connected(WorldTile tile1, int x, int y, TileData tile2)
        {
            Node node1 = new Node(tile1.Pos.x, tile1.Pos.y);
            node1.SetConnections(tile1.Data.connectsTop, tile1.Data.connectsRight, tile1.Data.connectsBottom, tile1.Data.connectsLeft);
            Node node2 = new Node(x, y);
            node2.SetConnections(tile2.connectsTop, tile2.connectsRight, tile2.connectsBottom, tile2.connectsLeft);
            return Node.Connected(node1, node2);
        }

        private bool InBounds(int xCell, int yCell)
        {
            return xCell >= 0 && xCell < _world.Length && yCell >= 0 && yCell < _world[xCell].Length;
        }
    }
}