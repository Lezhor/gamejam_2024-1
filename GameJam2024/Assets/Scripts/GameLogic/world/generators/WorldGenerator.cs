using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLogic.world.generators
{
    public abstract class WorldGenerator : ScriptableObject
    {
        public GameManager gameManager => GameManager.Instance;
        [Header("Settings")]
        public Vector2Int dimensions;


        // Abstract method to generate the world
        public abstract World GenerateWorld();
    }
}