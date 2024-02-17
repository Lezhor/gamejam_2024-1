using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    public abstract class TileActionFactoryWithHint : TileActionFactory
    {
        [Header("Hint")] public string hint;
    }
}