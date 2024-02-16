using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    [CreateAssetMenu(fileName = "New Debug Tile Action", menuName = "World/Actions/Debug Tile Action")]
    public class DebugTileActionFactory : TileActionFactory
    {
        [SerializeField] 
        private bool debugSetVisible = true;
        [SerializeField] 
        private bool debugSetExplored = true;
        [SerializeField] 
        private bool debugPlayerEnter = true;
        [SerializeField] 
        private bool debugPlayerExit = true;
        [SerializeField] 
        private bool debugPerformAction = true;
        
        public override TileAction CreateAction(WorldTile tile)
        {
            return new DebugTileAction(tile, debugSetVisible, debugSetExplored,
                debugPlayerEnter, debugPlayerExit, debugPerformAction);
        }
    }
}