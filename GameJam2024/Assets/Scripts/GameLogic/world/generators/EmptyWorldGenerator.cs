using UnityEngine;

namespace GameLogic.world.generators
{
    [CreateAssetMenu(fileName = "New Empty World Generator", menuName = "World Generator/Empty World Generator")]
    public class EmptyWorldGenerator : WorldGenerator
    {
        protected override Node[,] GenerateWorldGraph(Vector2Int size)
        {
            Node[,] graph = new Node[size.x, size.y];
            Vector2Int startPos = StartPos(size);
            graph[startPos.x, startPos.y] = new Node(startPos.x, startPos.y);
            return graph;
        }

        protected override TileData GetTile(Node node)
        {
            return node == null ? Registry.emptyTile : Registry.nwse;
        }

        protected override Vector2Int StartPos(Vector2Int size)
        {
            return new Vector2Int(Mathf.FloorToInt(size.x / 2f), Mathf.FloorToInt(size.y / 2f));
        }
    }
}