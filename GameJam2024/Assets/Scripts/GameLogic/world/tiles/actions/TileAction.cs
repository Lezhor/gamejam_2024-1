namespace GameLogic.world.tiles.actions
{
    public abstract class TileAction
    {

        public bool Executed { get; private set; }

        protected WorldTile Tile { get; }

        protected TileAction(WorldTile tile)
        {
            Tile = tile;
        }

        public abstract void OnSetVisibility(bool state);

        public abstract void OnSetExplored(bool state);

        public abstract void OnPlayerEnterTile();

        public abstract void OnPlayerExitTile();

        public void Invoke(PlayerController player)
        {
            if (!Executed)
            {
                if (PerformAction(player))
                {
                    Executed = true;
                    Tile.RedrawOnTilemaps();
                }
            }
        }
        
        protected abstract bool PerformAction(PlayerController player);
    }
}