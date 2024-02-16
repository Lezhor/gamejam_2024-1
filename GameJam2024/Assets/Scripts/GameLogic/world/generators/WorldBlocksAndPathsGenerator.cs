using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.world.tiles;
using GameLogic.world.tiles.actionTiles;
using math;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GameLogic.world.generators
{
    [CreateAssetMenu(fileName = "New World Blocks And Paths Generator", menuName = "World Generator/World Blocks And Paths Generator")]
    public class WorldBlocksAndPathsGenerator : WorldGenerator
    {

        public const int MaskBlocked = 0b0001;
        public const int MaskPath = 0b0010;
        public const int MaskDoor = 0b0100;

        [Header("Blocked Tiles Settings")]
        [SerializeField]
        [Range(0, 100)] 
        private int blockedTilesPercentage = 15;

        [SerializeField] 
        [Range(1, 9)] 
        private int maxBlockedTilesIn3x3Box = 2;
        [SerializeField] 
        [Range(1, 25)] 
        private int maxBlockedTilesIn5x5Box = 4;

        protected Vector2Int _startPos = new(0, 0);
        [Header("Path Settings")] 
        [SerializeField]
        [Range(0, 100)]
        private int pathTilePercentage = 10;
        [SerializeField] 
        [Range(0, 4)]
        private int minExitsPerBatch = 2;
        [SerializeField]
        [Range(0.5f, 3f)]
        private float minExitsPerPathBatchPercentage = 1.1f;
        [SerializeField]
        [Range(0.5f, 3f)]
        private float maxExitsPerPathBatchPercentage = 2f;
        
        [SerializeField] 
        private List<BatchGenerator> batchGenerators;

        private Node[,] _graph;

        protected override Node[,] GenerateWorldGraph(Vector2Int size)
        {
            _graph = CreateBasicGraph(size);
            
            _startPos = new Vector2Int(3, Mathf.FloorToInt(size.y / 2f));
            _graph[_startPos.x, _startPos.y].SetConnections(true, true, true, true);
            _graph[_startPos.x, _startPos.y].SetMask(MaskPath);

            int blockedTilesCount = (int)(size.x * size.y * blockedTilesPercentage / 100f);

            for (int i = 0; i < blockedTilesCount; i++)
            {
                Vector2Int pos;
                int counter = 0;
                do
                {
                    pos = new Vector2Int(Random.Range(0, _graph.GetLength(0)), Random.Range(0, _graph.GetLength(1)));
                    if (counter++ >= 100)
                    {
                        Debug.Log("Too many attempts to place Blocked Tile!!!");
                        break;
                    }
                } while (!BlockCanBePlaced(pos));

                if (counter <= 99)
                {
                    _graph[pos.x, pos.y].SetMask(MaskBlocked);
                }
                else
                {
                    Debug.Log("Only placed " + (i + 1) + " blocked Tiles out of " + blockedTilesCount);
                    break;
                }
            }

            GetNodesIn3X3Box(_graph, _startPos)
                .ForEach(node => node.UnsetMask(MaskBlocked));

            int countPathTiles = (int)(size.x * size.y * pathTilePercentage / 100f);

            int count = 0;
            while (count >= 0 && count < countPathTiles)
            {
                count += GeneratePathBatch(_graph);
            }

            return _graph;
        }
        
        private bool BlockCanBePlaced(Vector2Int pos)
        {
            if (_graph[pos.x, pos.y].Value != 0)
            {
                return false;
            }

            Node tempBlockedNode = new Node(pos.x, pos.y)
            {
                Value = MaskBlocked
            };
            if (GetNeighbours(_graph, pos).Where(node => node.IsMaskSet(MaskPath)).Any(node => !Node.ConnectionsMatch(node, tempBlockedNode)))
            {
                return false;
            }

            return GetNodesIn3X3Box(_graph, pos).Count(node => node.IsMaskSet(MaskBlocked)) < maxBlockedTilesIn3x3Box
                   && GetNodesIn5X5Box(_graph, pos).Count(node => node.IsMaskSet(MaskBlocked)) < maxBlockedTilesIn5x5Box;
        }

        private int GeneratePathBatch(Node[,] graph)
        {
            List<Vector2Int> batchCoords = new List<Vector2Int>();

            Vector2Int startPos;

            int counter = 0;
            do
            {
                startPos = new Vector2Int(Random.Range(0, graph.GetLength(0)), Random.Range(0, graph.GetLength(1)));
                if (counter++ >= 100)
                {
                    Debug.Log("Too many attempts to place Path Batch!!!");
                    break;
                }
            } while (!CanPlacePathHere(graph, startPos, true));

            if (counter >= 100)
            {
                return int.MaxValue;
            }
            
            batchCoords.Add(startPos);

            BatchGenerator batchGenerator = CustomMath.PickRandomOption(batchGenerators,
                batchGenerator => batchGenerator.importance);

            int tilesInBatch = batchGenerator.GetRandomBatchCount();

            for (int i = 1; i < tilesInBatch; i++)
            {
                // TODO Finish
                /*
                    1. Get Neighbours of all current Vector2Int which are not blocked, not from the list itself and satisfy CanPlacePathHere
                    2. Pick one randomly 
                    3. add to list
                 */
                List<Vector2Int> possibleExpansions = GetPossibleExtensions(graph, batchCoords, true);

                if (possibleExpansions.Count == 0)
                {
                    break;
                }

                Vector2Int nextExpansion = possibleExpansions[Random.Range(0, possibleExpansions.Count)];
                
                batchCoords.Add(nextExpansion);
            }
            
            batchCoords.ForEach(v => graph[v.x, v.y].SetMask(MaskPath));
            List<Node> batchNodes = batchCoords.Select(v => graph[v.x, v.y]).ToList();
            
            ConnectAllNodes(batchNodes);

            AddRandomExitsToPatch(graph, batchNodes);

            int doorCount = batchGenerator.GetRandomDoorCount();

            for (int i = 0; i < doorCount; i++)
            {
                if (!AddDoorToPathBatchIfPossible(batchNodes))
                {
                    Debug.Log("Couldn't add more doors do batch. Doors added: " + i);
                    break;
                }
            }

            return batchCoords.Count;
        }

        private void ConnectAllNodes(List<Node> batch)
        {
            /*
            for (int i = 0; i < batchNodes.Count; i++)
            {
                for (int k = i + 1; k < batchNodes.Count; k++)
                {
                    Node.Connect(batchNodes[i], batchNodes[k]);
                }
            }
            */

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(batch[Random.Range(0, batch.Count)]);
            
            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                current.Marked = true;
                List<Node> neighbours = batch
                    .Where(node => !node.Marked)
                    .Where(node => Node.Distance(current, node) == 1)
                    .Where(node => !Node.Connected(current, node))
                    .Where(node => !queue.Contains(node) || Random.Range(0, 3) == 0)
                    .ToList();
                neighbours.ForEach(node => Node.Connect(current, node));

                neighbours = neighbours.Where(node => !queue.Contains(node)).ToList();
                while (neighbours.Count > 0)
                {
                    Node node = neighbours[Random.Range(0, neighbours.Count)];
                    neighbours.Remove(node);
                    queue.Enqueue(node);
                }
            }

        }

        private void AddRandomExitsToPatch(Node[,] graph, List<Node> batch)
        {
            int exitCount = Math.Max(minExitsPerBatch,
                Random.Range(Mathf.CeilToInt(batch.Count * minExitsPerPathBatchPercentage),
                    Mathf.CeilToInt(batch.Count * maxExitsPerPathBatchPercentage)));

            List<Node> extensions = GetPossibleExtensions(graph, batch.Select(node => node.Pos).ToList(), false)
                .Select(v => graph[v.x, v.y]).ToList();

            for (int i = 0; i < exitCount; i++)
            {
                if (extensions.Count == 0)
                {
                    break;
                }

                Node next = extensions[Random.Range(0, extensions.Count)];
                extensions.Remove(next);
                
                OpenConnectionTo(graph, batch, next.Pos);
            }

        }

        private void OpenConnectionTo(Node[,] graph, List<Node> batch, Vector2Int pos)
        {
            List<Vector2Int> neighboursOfPos = Directions.Select(v => v + pos)
                .Where(v => batch.Select(n => n.Pos).Contains(v))
                .ToList();
            Vector2Int chosen = neighboursOfPos[Random.Range(0, neighboursOfPos.Count)];
            Node node = graph[chosen.x, chosen.y];
            Node temp = new Node(pos.x, pos.y);
            
            Node.Connect(node, temp);
        }

        private List<Vector2Int> GetPossibleExtensions(Node[,] graph, List<Vector2Int> batch, bool checkForSurroundingPaths)
        {
            List<Vector2Int> possibleExpansions = new List<Vector2Int>();
            foreach (Vector2Int v in batch)
            {
                foreach (Node node in GetNeighbours(graph, v))
                {
                    Vector2Int pos = node.Pos;
                    if (node.Value == 0 && !batch.Contains(pos) && !possibleExpansions.Contains(pos) && CanPlacePathHere(graph, pos, checkForSurroundingPaths))
                    {
                        possibleExpansions.Add(pos);
                    }
                }
            }

            return possibleExpansions;
        }

        private bool AddDoorToPathBatchIfPossible(List<Node> batch)
        {
            List<Node> nodesWith2Or3ConnectionsAndNoDoor = batch
                .Where(node => !node.IsMaskSet(MaskDoor))
                .Where(node => node.CountConnection() > 1 && node.CountConnection() < 4)
                .ToList();

            if (nodesWith2Or3ConnectionsAndNoDoor.Count == 0)
            {
                return false;
            }

            nodesWith2Or3ConnectionsAndNoDoor[Random.Range(0, nodesWith2Or3ConnectionsAndNoDoor.Count)].SetMask(MaskDoor);
            return true;
        }

        private bool CanPlacePathHere(Node[,] graph, Vector2Int pos, bool checkForSurroundingPaths)
        {
            return graph[pos.x, pos.y].Value == 0 
                   && (!checkForSurroundingPaths || 
                      GetNodesIn7x7Box(graph, pos).All(node => !node.IsMaskSet(MaskPath)));
        }

        protected override TileData GetTile(Node node)
        {
            if (node.IsMaskSet(MaskBlocked))
            {
                return Registry.blockedTile;
            }
            if (node.IsMaskSet(MaskPath))
            {
                return Registry.GetTile(node.Top, node.Right, node.Bottom, node.Left);
            }

            return Registry.emptyTile;
        }

        protected override ActionTile GetAction(Node node)
        {
            if (node.IsMaskSet(MaskDoor))
            {
                return ActionRegistry.doorTile;
            }
            // TODO - Add GoldActionTile
            return null;
        }

        protected override Vector2Int StartPos(Vector2Int size)
        {
            return _startPos;
        }

        [Serializable]
        public class BatchGenerator
        {
            [Header("Settings")]
            public float importance = 1;
            [FormerlySerializedAs("minPerBatch")] [SerializeField]
            int minPathsPerBatch = 1;
            [FormerlySerializedAs("maxPerBatch")] [SerializeField]
            int maxPathsPerBatch = 6;
            [SerializeField]
            int diceCount = 2;
            [SerializeField]
            private int minDoorsPerBatch = 0;
            [SerializeField]
            private int maxDoorsPerBatch = 1;

            public int GetRandomBatchCount()
            {
                int value = 0;
                for (int i = 0; i < diceCount; i++)
                {
                    value += Random.Range(0, maxPathsPerBatch + 1 - minPathsPerBatch);
                }

                return minPathsPerBatch + Mathf.CeilToInt(value / (float) diceCount);
            }

            public int GetRandomDoorCount()
            {
                return Random.Range(minDoorsPerBatch, maxDoorsPerBatch + 1);
            }

        }
    }
}