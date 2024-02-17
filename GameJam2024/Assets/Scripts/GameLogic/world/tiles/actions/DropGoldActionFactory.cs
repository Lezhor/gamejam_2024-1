using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    [CreateAssetMenu(fileName = "New Drop Gold Action", menuName = "World/Actions/Drop Gold Action")]
    public class DropGoldActionFactory : TileActionFactoryWithHint
    {
        [Header("Stats")]
        [SerializeField]
        private int minGold = 20;
        [SerializeField]
        private int maxGold = 50;
        [SerializeField]
        private int roundStep = 10;


        public override TileAction CreateAction(WorldTile tile)
        {
            return new DropGoldAction(tile, hint, minGold, maxGold, roundStep);
        }
    }
}