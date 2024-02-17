using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameLogic.world.tiles.actions
{
    public class DebugTileAction : TileAction
    {
        private readonly bool _debugSetVisible;
        private readonly bool _debugSetExplored;
        private readonly bool _debugPlayerEnter;
        private readonly bool _debugPlayerExit;
        private readonly bool _debugPerformAction;

        public string Tag => "DebugTile{ ( " + Tile.Pos.x + " | " + Tile.Pos.y + " ) }:  ";

        public DebugTileAction(WorldTile tile, bool debugSetVisible, bool debugSetExplored, bool debugPlayerEnter, bool debugPlayerExit, bool debugPerformAction) : base(tile)
        {
            _debugSetVisible = debugSetVisible;
            _debugSetExplored = debugSetExplored;
            _debugPlayerEnter = debugPlayerEnter;
            _debugPlayerExit = debugPlayerExit;
            _debugPerformAction = debugPerformAction;
        }

        public override void OnSetVisibility(bool state)
        {
            if (_debugSetVisible)
            {
                Debug.Log(Tag + "Visibility set to " + state);
            }
        }

        public override void OnSetExplored(bool state)
        {
            if (_debugSetExplored)
            {
                Debug.Log(Tag + "Explored set to " + state);
            }
        }

        public override void OnPlayerEnterTile()
        {
            if (_debugPlayerEnter)
            {
                Debug.Log(Tag + "Player Entered Tile!");
            }
        }

        public override void OnPlayerExitTile()
        {
            if (_debugPlayerExit)
            {
                Debug.Log(Tag + "Player Exited Tile!");
            }
        }

        protected override bool PerformAction(PlayerController player)
        {
            if (_debugPerformAction)
            {
                Debug.Log(Tag + "Action performed!");
            }

            return true;
        }
    }
}