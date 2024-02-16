using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    public abstract class TileAction : ScriptableObject
    {
        // TODO - Add Actions for pickup gold, release Monster etc.

        public bool Executed { get; protected set; } = false;

        // OnBecomeVisible
        // OnExplore
        // OnPerformAction

    }
}