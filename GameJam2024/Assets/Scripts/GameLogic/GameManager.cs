using System;
using audio;
using GameLogic.world;
using GameLogic.world.generators;
using GameLogic.world.tiles.actionTiles;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace GameLogic
{
    public class GameManager : MonoBehaviour
    {
        [Header("Settings")] 
        public WorldGenerator worldGenerator;
        public int worldBorderWidth = 10;
        [SerializeField]
        private bool spectatorMode = false;

        private bool _spectatorModeLastFrame = false;

        public bool SpectatorMode => spectatorMode;

        [Header("Other")] 
        [SerializeField] private GameObject _player;
        public GameObject Player => _player;
        public PlayerController PlayerScript => _player.GetComponent<PlayerController>();
        [SerializeField] private Camera _camera;
        [SerializeField] private MessageManager messageManager;
        public MessageManager MessageManager => messageManager;
        [SerializeField] private AudioManager audioManager;
        public AudioManager AudioManager => audioManager;
        public Camera Cam => _camera;

        [Header("Tilemaps")] public Tilemap background;
        [FormerlySerializedAs("smoke")] public Tilemap fog;
        public Tilemap fogPath;
        public Tilemap walls;
        public Tilemap foreground;

        [Header("Resources")] 
        [SerializeField] private TileRegistry _tiles;
        [SerializeField] private ActionTileRegistry _actionTiles;


        private static GameManager _instance;
        public static GameManager Instance => _instance;

        public TileRegistry Tiles => _tiles;
        public ActionTileRegistry ActionTiles => _actionTiles;

        public TilePlaceEventRegistry PlaceEvents { get; } = new();

        private World _world;
        public World World => _world;

        private void Awake()
        {
            _instance = this;

            _spectatorModeLastFrame = spectatorMode;

            // TODO - WorldGenerator should create World
            _world = worldGenerator.GenerateWorld();
            /*
        Vector2Int startPos = new(1, Mathf.CeilToInt(size.y / 2f));
        _world = new World(size, startPos, this);
        */
        }

        private void Start()
        {
            _player.transform.position = new Vector3(_world.StartPos.x + 0.5f, _world.StartPos.y + 0.5f);
        }

        private void Update()
        {
            if (spectatorMode != _spectatorModeLastFrame)
            {
                _spectatorModeLastFrame = spectatorMode;
                Debug.Log("Redrawing all Tiles!!!");
                _world.RedrawAllTiles();
            }
        }
    }
}