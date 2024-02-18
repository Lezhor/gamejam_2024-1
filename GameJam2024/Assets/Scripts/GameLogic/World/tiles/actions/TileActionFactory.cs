using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    public abstract class TileActionFactory : ScriptableObject
    {
        [SerializeField]
        protected string soundToPlayOnAction;
        public abstract TileAction CreateAction(WorldTile tile);
    }
}