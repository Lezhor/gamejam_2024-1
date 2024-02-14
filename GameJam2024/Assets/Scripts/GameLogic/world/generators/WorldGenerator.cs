using UnityEngine;

namespace GameLogic.world.generators
{
    public abstract class WorldGenerator : ScriptableObject
    {
        public GameManager GameManager => GameManager.Instance;
        public TileRegistry Registry => GameManager.Tiles;
        [Header("Settings")]
        [SerializeField]
        private Vector2Int dimensions;

        protected abstract Node[,] GenerateWorldGraph(Vector2Int size);

        // method to generate the world
        public World GenerateWorld()
        {
            Debug.Log("Generating world");
            Node[,] graph = GenerateWorldGraph(dimensions);
            WorldTile[][] board = new WorldTile[graph.GetLength(0)][];
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = new WorldTile[graph.GetLength(1)];
            }

            Debug.Log("Filling tiles");
            for (int x = 0; x < board.Length; x++)
            {
                for (int y = 0; y < board[x].Length; y++)
                {
                    board[x][y] = new WorldTile(GetTile(graph[x, y]), x, y, GameManager);
                }
            }

            Debug.Log("World generated!");

            return new World(board, StartPos(dimensions), GameManager);
        }

        protected abstract TileData GetTile(Node node);

        protected abstract Vector2Int StartPos(Vector2Int size);
    }
}