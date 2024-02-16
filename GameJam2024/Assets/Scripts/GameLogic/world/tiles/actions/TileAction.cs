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

        public void Invoke()
        {
            PerformAction();
            Executed = true;
            Tile.RedrawOnTilemaps();
        }
        
        protected abstract void PerformAction();
    }
}