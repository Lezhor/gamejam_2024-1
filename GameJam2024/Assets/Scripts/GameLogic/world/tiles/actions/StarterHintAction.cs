namespace GameLogic.world.tiles.actions
{
    public class StarterHintAction : TileActionWithHint
    {
        public StarterHintAction(WorldTile tile, string hint) : base(tile, "", hint)
        {
        }

        public override void OnSetVisibility(bool state)
        {
        }

        public override void OnSetExplored(bool state)
        {
        }

        protected override bool PerformAction(PlayerController player)
        {
            return false;
        }
    }
}