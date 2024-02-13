using UnityEngine;

namespace GameLogic.world.generators
{
    [CreateAssetMenu(fileName = "New Empty World Generator", menuName = "World Generator/Empty World Generator")]
    public class EmptyWorldGenerator : WorldGenerator
    {
        public override World GenerateWorld()
        {
            return new World(dimensions, new Vector2Int(Mathf.FloorToInt(dimensions.x / 2f), Mathf.FloorToInt(dimensions.y / 2f)), gameManager);
        }
    }
}