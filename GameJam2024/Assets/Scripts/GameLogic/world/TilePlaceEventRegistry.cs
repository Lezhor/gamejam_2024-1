using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.world
{
    public class TilePlaceEventRegistry
    {
        public event Action<Vector2Int> OnTriedToPlaceTileToFar;
        public void InvokeTriedToPlaceTileToFar(Vector2Int pos) => OnTriedToPlaceTileToFar?.Invoke(pos);

        /**
         * First param: Tile which failed to be placed
         * Second param: TileData of the tile
         * Third param: List of tiles which were the reason for this.
         */
        public event Action<Vector2Int, TileData, List<Vector2Int>> OnPlacementFailed;
        
        public void InvokePlacementFailed(Vector2Int pos, TileData tile, List<Vector2Int> reasonTiles)
            => OnPlacementFailed?.Invoke(pos, tile, reasonTiles);
    }
}