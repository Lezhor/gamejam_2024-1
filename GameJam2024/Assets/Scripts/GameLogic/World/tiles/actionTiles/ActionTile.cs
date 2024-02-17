using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.world.generators;
using GameLogic.world.tiles.actions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameLogic.world.tiles
{
    [CreateAssetMenu(fileName = "New Action Tile", menuName = "World/Action Tile")]
    public class ActionTile : ScriptableObject
    {

        public List<TileActionFactory> possibleActions;

        public List<Variant> possibleTiles;

        public List<Variant> GetFittingVariants(TileData tile)
        {
            return possibleTiles.Where(variant => variant.FitsTile(tile)).ToList();
        }

        public bool HasFittingVariants(Node tile)
        {
            return possibleTiles.Count(variant => variant.FitsTile(tile)) > 0;
        }

        [Serializable]
        public class Variant
        {
            public TileBase tileBeforeAction;
            public TileBase tileAfterAction;

            [SerializeField]
            private bool constraintsShouldMatch = true;

            [SerializeField]
            private List<TileData> listOfConstraints;

            public TileBase GetTile(bool actionExecuted) => actionExecuted ? tileAfterAction : tileBeforeAction;

            public bool FitsTile(TileData tile)
            {
                if (constraintsShouldMatch)
                {
                    return listOfConstraints.Any(otherTile => TilesMatch(tile, otherTile));
                }
                else
                {
                    return listOfConstraints.TrueForAll(otherTile => !TilesMatch(tile, otherTile));
                }
            }
            public bool FitsTile(Node tile)
            {
                if (constraintsShouldMatch)
                {
                    return listOfConstraints.Any(otherTile => TilesMatch(tile, otherTile));
                }
                else
                {
                    return listOfConstraints.TrueForAll(otherTile => !TilesMatch(tile, otherTile));
                }
            }

            private bool TilesMatch(TileData tile1, TileData tile2)
            {
                return tile1.connectsTop == tile2.connectsTop
                       && tile1.connectsLeft == tile2.connectsLeft
                       && tile1.connectsBottom == tile2.connectsBottom
                       && tile1.connectsRight == tile2.connectsRight;
            }
            
            private bool TilesMatch(Node tile1, TileData tile2)
            {
                return tile1.Top == tile2.connectsTop
                       && tile1.Left == tile2.connectsLeft
                       && tile1.Bottom == tile2.connectsBottom
                       && tile1.Right == tile2.connectsRight;
            }
        }

    }
}
