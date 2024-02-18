using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    public class GameOverAction : TileActionWithHint
    {
        private readonly bool _won;
        
        public GameOverAction(WorldTile tile, string sound, string hint, bool won) : base(tile, sound, hint)
        {
            _won = won;
        }

        public override void OnSetVisibility(bool state)
        {
        }

        public override void OnSetExplored(bool state)
        {
        }

        protected override bool PerformAction(PlayerController player)
        {
            // TODO - End Game! - Event
            if (_won)
            {
                Debug.Log("Player won game!!!");
                GameManager.Instance.UIManager.ShowWinScreen();
            }
            else
            {
                Debug.Log("Player lost game!!!");
            }

            return true;
        }
    }
}