using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    public abstract class TileActionFactory : ScriptableObject
    {
        public abstract TileAction CreateAction(WorldTile tile);
    }
}