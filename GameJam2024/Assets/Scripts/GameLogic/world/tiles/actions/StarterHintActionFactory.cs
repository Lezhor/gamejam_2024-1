using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    [CreateAssetMenu(fileName = "New Starter Hint Action", menuName = "World/Actions/Starter Hint Action")]
    public class StarterHintActionFactory : TileActionFactoryWithHint
    {
        public override TileAction CreateAction(WorldTile tile)
        {
            return new StarterHintAction(tile, hint);
        }
    }
}