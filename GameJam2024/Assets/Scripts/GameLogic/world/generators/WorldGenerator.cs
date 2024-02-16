using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.world.tiles;
using GameLogic.world.tiles.actionTiles;
using UnityEngine;

namespace GameLogic.world.generators
{
    public abstract class WorldGenerator : ScriptableObject
    {

        protected static readonly List<Vector2Int> Directions = new(new[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
        });
        
        public GameManager GameManager => GameManager.Instance;
        public TileRegistry Registry => GameManager.Tiles;
        public ActionTileRegistry ActionRegistry => GameManager.ActionTiles;
        [Header("General Settings")]
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
                    board[x][y] = new WorldTile(GetTile(graph[x, y]), GetAction(graph[x, y]), x, y, GameManager);
                }
            }

            Debug.Log("World generated!");

            return new World(board, StartPos(dimensions), GameManager);
        }

        protected abstract TileData GetTile(Node node);

        /**
         * Should return null for no action
         */
        protected abstract ActionTile GetAction(Node node);

        protected abstract Vector2Int StartPos(Vector2Int size);

        /**
         * Creates graph with size size and sets each node.
         */
        protected Node[,] CreateBasicGraph(Vector2Int size)
        {
            Node[,] graph = new Node[size.x, size.y];
            FillNodes(graph);
            return graph;
        }

        protected void FillNodes(Node[,] graph)
        {
            FillNodes(graph, (x, y) => new Node(x, y));
        }

        protected void FillNodes(Node[,] graph, Func<int, int, Node> nodeFactory)
        {
            for (int y = 0; y < graph.GetLength(1); y++)
            {
                for (int x = 0; x < graph.GetLength(0); x++)
                {
                    graph[x, y] = nodeFactory.Invoke(x, y);
                }
            }
        }

        protected void ForeachNode(Node[,] graph, Action<Node> action)
        {
            for (int y = 0; y < graph.GetLength(1); y++)
            {
                for (int x = 0; x < graph.GetLength(0); x++)
                {
                    action.Invoke(graph[x, y]);
                }
            }
        }

        protected List<Node> GetNeighbours(Node[,] graph, Vector2Int pos)
        {
            return Directions
                .Select(v => pos + v)
                .Where(v => InBounds(graph, v))
                .Select(v => graph[v.x, v.y])
                .ToList();
        }
        
        protected List<Node> GetNodesIn3X3Box(Node[,] graph, Vector2Int pos)
        {
            return GetNodesInBox(graph, pos, 1);
        }

        protected List<Node> GetNodesIn5X5Box(Node[,] graph, Vector2Int pos)
        {
            return GetNodesInBox(graph, pos, 2);
        }

        protected List<Node> GetNodesInBox(Node[,] graph, Vector2Int pos, int radius)
        {
            List<Node> list = new List<Node>();

            for (int y = pos.y - radius; y <= pos.y + radius; y++)
            {
                for (int x = pos.x - radius; x <= pos.x + radius; x++)
                {
                    if (InBounds(graph, x, y) && (x != pos.x || y != pos.y))
                    {
                        list.Add(graph[x, y]);
                    }
                }
            }

            return list;
        }

        private bool InBounds(Node[,] graph, Vector2Int pos)
        {
            return InBounds(graph, pos.x, pos.y);
        }

        private bool InBounds(Node[,] graph, int x, int y)
        {
            return x >= 0 && x < graph.GetLength(0) && y >= 0 && y < graph.GetLength(1);
        }
    }
}