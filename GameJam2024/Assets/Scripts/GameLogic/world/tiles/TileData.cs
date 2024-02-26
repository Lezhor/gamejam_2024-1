using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace GameLogic.world
{
    [CreateAssetMenu(fileName = "New Tile Data", menuName = "World/Tile Data")]
    public class TileData : ScriptableObject
    {
        [Header("Sprites")]
        public Sprite sprite;

        [Header("Tilemap Resources")] 
        public TileBase imageInvis;
        [FormerlySerializedAs("imageSmoke")] public TileBase imageFogInvis;
        [FormerlySerializedAs("imageFogDark")] public TileBase imageFogPath;
        public TileBase imageFloor;
        public TileBase imageWalls;
        public TileBase imagePlaceHint;
    
        [Header("Connections")]
        public bool connectsTop = false;
        public bool connectsRight = false;
        public bool connectsBottom = false;
        public bool connectsLeft = false;

        [Header("Constraints")]
        public bool mustConnect = true;
        public bool diggable = false;
        public bool bombable = true;

        public override string ToString()
        {
            return "Tile-"
                   + (connectsTop ? "N" : "")
                   + (connectsLeft ? "W" : "")
                   + (connectsBottom ? "S" : "")
                   + (connectsRight ? "E" : "");
        }

        public bool HasConnections()
        {
            return connectsTop || connectsRight || connectsBottom || connectsLeft;
        }
    }
}
