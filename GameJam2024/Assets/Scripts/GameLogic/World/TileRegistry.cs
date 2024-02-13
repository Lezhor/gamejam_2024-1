using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLogic
{
    [Serializable]
    public class TileRegistry
    {
        [Header("Non-Path-Tiles")]
        public TileData emptyTile;
        public TileData blockedTile;

        [Header("Dead Ends")] 
        public TileData n;
        public TileData w;
        public TileData s;
        public TileData e;

        [Header("Straights")]
        [FormerlySerializedAs("tile_we")] public TileData we;
        [FormerlySerializedAs("tile_ns")] public TileData ns;
        [Header("Turns")] 
        [FormerlySerializedAs("tile_nw")] public TileData nw;
        [FormerlySerializedAs("tile_ws")] public TileData ws;
        [FormerlySerializedAs("tile_se")] public TileData se;
        [FormerlySerializedAs("tile_ne")] public TileData ne;
        [Header("T-Shapes")] 
        [FormerlySerializedAs("tile_nws")] public TileData nws;
        [FormerlySerializedAs("tile_wse")] public TileData wse;
        [FormerlySerializedAs("tile_nse")] public TileData nse;
        [FormerlySerializedAs("tile_nwe")] public TileData nwe;
        [Header("Cross-Intersection")]
        [FormerlySerializedAs("tile_nwse")] public TileData nwse;

        public TileData GetTile(bool top, bool right, bool bottom, bool left)
        {
            return GetTile((top ? "n" : "") + (left ? "w" : "") + (bottom ? "s" : "") + (right ? "e" : ""));
        }

        public TileData GetTile(string str)
        {
            return str switch
            {
                "n" => n,
                "w" => w,
                "s" => s,
                "e" => e,
                "we" => we,
                "ns" => ns,
                "nw" => nw,
                "ws" => ws,
                "se" => se,
                "ne" => ne,
                "nws" => nws,
                "wse" => wse,
                "nse" => nse,
                "nwe" => nwe,
                "nwse" => nwse,
                _ => emptyTile
            };
        }
        
    }
}