using UnityEngine;

namespace GameLogic.world.tiles.actions
{
    [CreateAssetMenu(fileName = "New Game Over Action", menuName = "World/Actions/Game Over Action")]
    public class GameOverActionFactory : TileActionFactoryWithHint
    {
        [Header("Stats")]
        public bool won;
        
        public override TileAction CreateAction(WorldTile tile)
        {
            return new GameOverAction(tile, soundToPlayOnAction, hint, won);
        }
    }
}