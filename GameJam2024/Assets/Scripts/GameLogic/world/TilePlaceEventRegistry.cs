using System;
using UnityEngine;

namespace GameLogic.world
{
    public class TilePlaceEventRegistry
    {
        public event Action<Vector2Int> OnTriedToPlaceTileToFar;
        public void InvokeTriedToPlaceTileToFar(Vector2Int pos) => OnTriedToPlaceTileToFar?.Invoke(pos);
    }
}