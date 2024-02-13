using GameLogic.world;
using UnityEngine;

namespace GameLogic.player
{
    public class TileRandomizer
    {
        private readonly TileRegistry _registry;
        
        public TileRandomizer(TileRegistry registry)
        {
            _registry = registry;
        }

        /**
         * Random tile with equal probabilities (No Dead Ends)
         */
        public TileData GetRandomTile()
        {
            return Random.Range(0, 11) switch
            {
                0 => _registry.we,
                1 => _registry.ns,
                
                2 => _registry.nw,
                3 => _registry.ws,
                4 => _registry.se,
                5 => _registry.ne,
                
                6 => _registry.nws,
                7 => _registry.wse,
                8 => _registry.nse,
                9 => _registry.nwe,
                
                10 => _registry.nwse,
                _ => null
            };
        }
    }
}