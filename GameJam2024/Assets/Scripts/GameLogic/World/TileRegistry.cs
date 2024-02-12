using System;
using UnityEngine;

namespace GameLogic
{
    [Serializable]
    public class TileRegistry
    {
        [Header("Non-Path-Tiles")]
        public TileData emptyTile;
        public TileData blockedTile;

        [Header("Straights")]
        public TileData tile_we;
        public TileData tile_ns;
        [Header("Turns")] 
        public TileData tile_nw;
        public TileData tile_ws;
        public TileData tile_se;
        public TileData tile_ne;
        [Header("T-Shapes")] 
        public TileData tile_nws;
        public TileData tile_wse;
        public TileData tile_nse;
        public TileData tile_nwe;
        [Header("Cross-Intersection")]
        public TileData tile_nwse;
    }
}