using System;
using Random = UnityEngine.Random;

namespace GameLogic.world.tiles.actions
{
    public class DropGoldAction : TileActionWithHint
    {
        
        private readonly int _minGold;
        private readonly int _maxGold;
        private readonly int _roundStep;
        
        public DropGoldAction(WorldTile tile, string hint, int minGold, int maxGold, int roundStep) : base(tile, hint)
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

        protected override bool PerformAction(PlayerController player)
        {
            int gold = GetGold();
            player.PlayerInventory.Gold += gold;
            GameManager.Instance.MessageManager.InvokeMessage(Tile.Center, "+" + gold + " Gold", true);
            return true;
        }

        private int GetGold()
        {
            int randomValue = Random.Range(0, (_maxGold - _minGold) / _roundStep + 1);

            return Math.Clamp(_minGold + randomValue * _roundStep, _minGold, _maxGold);
        }
    }
}