using System;
using Random = UnityEngine.Random;

namespace GameLogic.world.tiles.actions
{
    public class DropGoldAction : TileAction
    {
        
        private readonly int _minGold;
        private readonly int _maxGold;
        private readonly int _roundStep;
        
        public DropGoldAction(WorldTile tile, int minGold, int maxGold, int roundStep) : base(tile)
        {
            _minGold = minGold;
            _maxGold = maxGold;
            _roundStep = roundStep;
        }

        public override void OnSetVisibility(bool state)
        {
        }

        public override void OnSetExplored(bool state)
        {
        }

        public override void OnPlayerEnterTile(PlayerController player)
        {
        }

        public override void OnPlayerExitTile(PlayerController player)
        {
        }

        protected override bool PerformAction(PlayerController player)
        {
            player.PlayerInventory.Gold += GetGold();
            return true;
        }

        private int GetGold()
        {
            int randomValue = Random.Range(0, (_maxGold - _minGold) / _roundStep + 1);

            return Math.Clamp(_minGold + randomValue * _roundStep, _minGold, _maxGold);
        }
    }
}