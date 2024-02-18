namespace GameLogic.world.tiles.actions
{
    public abstract class TileAction
    {

        public bool Executed { get; private set; }

        protected WorldTile Tile { get; }

        private readonly string _soundPlayed;

        protected TileAction(WorldTile tile, string soundPlayed)
        {
            Tile = tile;
            _soundPlayed = soundPlayed;
        }

        public abstract void OnSetVisibility(bool state);

        public abstract void OnSetExplored(bool state);

        public abstract void OnPlayerEnterTile(PlayerController player);

        public abstract void OnPlayerExitTile(PlayerController player);

        public void Invoke(PlayerController player)
        {
            if (!Executed)
            {
                BeforeAction(player);
                if (PerformAction(player))
                {
                    if (_soundPlayed != null)
                    {
                        GameManager.Instance.AudioManager.Play(_soundPlayed);
                    }
                    Executed = true;
                    Tile.RedrawOnTilemaps();
                }
            }
        }
        
        protected abstract bool PerformAction(PlayerController player);

        protected virtual void BeforeAction(PlayerController player)
        {
        }
    }
}