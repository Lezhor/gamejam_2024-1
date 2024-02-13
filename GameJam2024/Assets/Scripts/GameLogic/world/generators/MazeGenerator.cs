using System.Collections.Generic;
using System.Linq;
using GameLogic.world.generators;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLogic.Maze
{
    [CreateAssetMenu(fileName = "New Maze Generator", menuName = "World Generator/Maze Generator")]
    public class MazeGenerator : WorldGenerator
    {
        private Node[,] _maze;

        private TileRegistry Registry => gameManager.Tiles;
        public Vector2Int startPos;

        public void GenerateMaze()
        {
            _maze = new Node[dimensions.x, dimensions.y];
            startPos = new Vector2Int(0, dimensions.y - 1);
            for (int y = 0; y < _maze.GetLength(1); y++)
            {
                for (int x = 0; x < _maze.GetLength(0); x++)
                {
                    _maze[x, y] = new Node(x, y);
                }
            }

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(_maze[startPos.x, startPos.y]);
            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                List<Node> neighbours = GetNotMarkedNeighbours(current);
                if (neighbours.Count > 0)
                {
                    Node neighbour = neighbours[Random.Range(0, neighbours.Count)];
                    Connect(current, neighbour);
                    neighbour.Marked = true;
                    AddNodeAtRandomPosition(queue, neighbour);
                    if (neighbours.Count > 1)
                    {
                        AddNodeAtRandomPosition(queue, current);
                    }
                }
            }
        }

        public override World GenerateWorld()
        {
            GenerateMaze();
            Debug.Log("Generating world");
            WorldTile[][] board = new WorldTile[_maze.GetLength(0)][];
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = new WorldTile[_maze.GetLength(1)];
            }

            Debug.Log("Filling tiles");
            for (int x = 0; x < board.Length; x++)
            {
                for (int y = 0; y < board[x].Length; y++)
                {
                    Node n = _maze[x, y];
                    board[x][y] = new WorldTile(Registry.GetTile(n.Top, n.Right, n.Bottom, n.Left), x, y, gameManager);
                }
            }

            Debug.Log("World generated!");

            return new World(board, startPos, gameManager);
        }

        private void Connect(Node node1, Node node2)
        {
            if (node1.X != node2.X && node1.Y != node2.Y)
            {
                Debug.Log("Can't connect nodes!");
            }

            if (node1.X > node2.X)
            {
                (node1, node2) = (node2, node1);
            }
            else if (node1.Y > node2.Y)
            {
                (node1, node2) = (node2, node1);
            }

            if (node2.X == node1.X + 1)
            {
                node1.Right = true;
                node2.Left = true;
            }
            else if (node2.Y == node1.Y + 1)
            {
                node1.Top = true;
                node2.Bottom = true;
            }
        }

        private List<Node> GetNotMarkedNeighbours(Node node)
        {
            List<Vector2Int> list = new List<Vector2Int>(new Vector2Int[]
            {
                new(node.X, node.Y + 1),
                new(node.X + 1, node.Y),
                new(node.X, node.Y - 1),
                new(node.X - 1, node.Y),
            });

            return list.Where(InBounds)
                .Select(v => _maze[v.x, v.y])
                .Where(n => !n.Marked)
                .ToList();
        }

        // Add a node at a random position within the queue
        public static void AddNodeAtRandomPosition<T>(Queue<T> queue, T newNode)
        {
            // Dequeue random number of elements from the beginning of the queue
            int dequeueCount = Random.Range(0, queue.Count + 1);
            for (int i = 0; i < dequeueCount; i++)
            {
                T dequeuedItem = queue.Dequeue();
                queue.Enqueue(dequeuedItem);
            }

            // Enqueue the new node
            queue.Enqueue(newNode);
        }

        private bool InBounds(Vector2Int v)
        {
            return InBounds(v.x, v.y);
        }

        private bool InBounds(int xCell, int yCell)
        {
            return xCell >= 0 && xCell < _maze.GetLength(0) && yCell >= 0 && yCell < _maze.GetLength(1);
        }

        private class Node
        {
            public int X;
            public int Y;

            public bool Marked;
            public bool Top;
            public bool Right;
            public bool Bottom;
            public bool Left;

            public Node(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}