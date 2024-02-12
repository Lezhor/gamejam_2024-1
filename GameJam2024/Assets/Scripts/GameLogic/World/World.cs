using UnityEngine;

namespace GameLogic
{
    public class World
    {
        private GameManager _gameManager;
        private TileRegistry Registry => _gameManager.Tiles;
        private WorldTile[][] world;

        public World(Vector2Int size, GameManager gameManager)
        {
            _gameManager = gameManager;

            world = new WorldTile[size.x][];
            for (int i = 0; i < world.Length; i++)
            {
                world[i] = new WorldTile[size.y];
                for (int k = 0; k < world[i].Length; k++)
                {
                    place(i, k, Registry.emptyTile);
                }
            }
        }


        /**
         * Returns true if placed successfully
         */
        public bool placeIfPossible(int xCell, int yCell, TileData tile)
        {
            if (!canBePlaced(xCell, yCell, tile))
                return false;
            place(xCell, yCell, tile);
            return true;
        }

        private void place(int x, int y, TileData tile)
        {
            world[x][y] = new WorldTile(tile, x, y, _gameManager);
        }

        private bool canBePlaced(int xCell, int yCell, TileData tile)
        {
            if (!tile.mustConnect)
            {
                return false;
            }

            if (!world[xCell][yCell].Data.destroyable)
            {
                return false;
            }

            if (inBounds(xCell, yCell + 1)
                && world[xCell][yCell + 1].Data.mustConnect
                && world[xCell][yCell + 1].Data.connectsBottom != tile.connectsTop)
            {
                return false;
            }

            if (inBounds(xCell + 1, yCell)
                && world[xCell + 1][yCell].Data.mustConnect
                && world[xCell + 1][yCell].Data.connectsLeft != tile.connectsRight)
            {
                return false;
            }

            if (inBounds(xCell, yCell - 1)
                && world[xCell][yCell - 1].Data.mustConnect
                && world[xCell][yCell - 1].Data.connectsTop != tile.connectsBottom)
            {
                return false;
            }

            if (inBounds(xCell - 1, yCell)
                && world[xCell - 1][yCell].Data.mustConnect
                && world[xCell - 1][yCell].Data.connectsRight != tile.connectsBottom)
            {
                return false;
            }

            return true;
        }

        private bool inBounds(int xCell, int yCell)
        {
            return xCell >= 0 && xCell < world.Length && yCell >= 0 && yCell < world[xCell].Length;
        }
    }
}