using System;
using System.Collections;
using Entities;
using GameLogic;
using GameLogic.player;
using GameLogic.world;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : EntityController
{
    
    public event Action<Vector2Int, Vector2Int> OnMovedToNewTile;
    public event Action<Vector2Int> OnActionKeyPressed;

    private Vector2Int _tilePosLastFrame;
    
    
    private PlayerInventory _playerInventory;

    public PlayerInventory PlayerInventory => _playerInventory;

    [Header("Player Settings")]
    [SerializeField]
    private float maxDigDistanceHorizontal = 1.5f;
    [SerializeField]
    private float maxDigDistanceVertical = 1f;
    [SerializeField]
    private int startGold = 300;

    private Camera _cam;
    private World _world;

    private GameManager _gameManager;
    private static readonly int Mine = Animator.StringToHash("Mine");

    public bool InputMouseClickEnabled { get; private set; } = true;
    public bool InputMouseMoveEnabled { get; private set; } = true;
    public bool InputMoveEnabled { get; private set; }  = true;
    public bool InputActionEnabled { get; private set; }  = true;
    public bool InputSelectInventoryEnabled { get; private set; }  = true;

    public void DisableInput(bool mouseClick, bool mouseMove, bool move, bool action, bool selectInv, float time)
    {
        DisableInput(mouseClick, mouseMove, move, action, selectInv);
        DoAfterDelay(() => EnableInput(mouseClick, mouseMove, move, action, selectInv), time);
    }

    public void DisableInput(bool mouseClick, bool mouseMove, bool move, bool action, bool selectInv)
    {
        if (mouseClick)
            InputMouseClickEnabled = false;
        if (mouseMove)
            InputMouseMoveEnabled = false;
        if (move)
            InputMoveEnabled = false;
        if (selectInv)
            InputSelectInventoryEnabled = false;
        if (action)
            InputActionEnabled = false;
    }

    public void EnableInput(bool mouseClick, bool mouseMove, bool move, bool action, bool selectInv)
    {
        if (mouseClick)
            InputMouseClickEnabled = true;
        if (mouseMove)
            InputMouseMoveEnabled = true;
        if (move)
            InputMoveEnabled = true;
        if (selectInv)
            InputSelectInventoryEnabled = true;
        if (action)
            InputActionEnabled = true;
    }

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _cam = _gameManager.Cam;
        _world = _gameManager.World;
        _tilePosLastFrame = GetTilePos(transform.position);
        TileRandomizer tileRandomizer = new TileRandomizer(_gameManager.Tiles);
        _playerInventory = new PlayerInventory(tileRandomizer)
        {
            Gold = startGold
        };
    }

    protected override void FixedUpdateAddOn()
    {
    }

    private void Update()
    {
        CheckForWASDInput();
        PlaceTileIfClicked();
        CheckIfMovedToNewTile();
        CheckIfActionKeyPressed();
        CheckForInvSlotChange();
        
        /*
        if (Input.GetKeyDown(KeyCode.U)) _playerInventory.Gold += 20;
        if (Input.GetKeyDown(KeyCode.I)) _playerInventory.Gold += 100;
        if (Input.GetKeyDown(KeyCode.O)) _playerInventory.Gold += 200;
        if (Input.GetKeyDown(KeyCode.P)) _playerInventory.Gold += 1000;
        */
    }

    private void CheckForWASDInput()
    {
        if (InputMoveEnabled)
        {
            MoveVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            MoveVector = new(0, 0);
        }
    }

    private void CheckIfMovedToNewTile()
    {
        Vector2Int tilePos = GetTilePos(transform.position);
        if (!tilePos.Equals(_tilePosLastFrame))
        {
            OnMovedToNewTile?.Invoke(_tilePosLastFrame, tilePos);
            _tilePosLastFrame = tilePos;
        }
    }

    private void CheckIfActionKeyPressed()
    {
        if (InputActionEnabled && Input.GetKeyDown(KeyCode.E))
        {
            OnActionKeyPressed?.Invoke(GetTilePos(transform.position));
        }
    }

    private Vector2Int GetTilePos(Vector3 pos)
    {
        return new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }

    private void PlaceTileIfClicked()
    {
        if (InputMouseClickEnabled && !_gameManager.UIManager.IsMouseOverUI() && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mousePos);
            Vector2Int tilePos = new Vector2Int(Mathf.FloorToInt(mouseWorldPos.x), Mathf.FloorToInt(mouseWorldPos.y));

            Vector3 tileWorldPos = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f);

            Vector3 distanceToTile = tileWorldPos - transform.position;

            distanceToTile = new Vector3(distanceToTile.x / maxDigDistanceHorizontal,
                distanceToTile.y / maxDigDistanceVertical);
            

            if (distanceToTile.magnitude > 1)
            {
                _gameManager.PlaceEvents?.InvokeTriedToPlaceTileToFar(tilePos);
                return;
            }

            if (_world.CanBePlaced(tilePos.x, tilePos.y, _playerInventory.CurrentSlot))
            {
                _gameManager.AudioManager?.Play("Dig", .1f);
                Animator?.SetTrigger(Mine);
                FlipDirection(distanceToTile.x >= 0);
                DisableInput(true, false, true, true, true);
                DoAfterDelay(() =>
                {
                    EnableInput(true, false, true, true, true);
                    _world.PlaceIfPossible(tilePos.x, tilePos.y, _playerInventory.CurrentSlot);
                    _playerInventory.ReplaceCurrentSlot();
                    _playerInventory.Gold -= 10;
                    _gameManager.MessageManager.InvokeMessage(new Vector2(tileWorldPos.x, tileWorldPos.y), "-10 Gold", false);
                }, .9f);
            }
        }
    }

    private void DoAfterDelay(Action action, float delay)
    {
        StartCoroutine(DoAfterDelayIEnumerator(action, delay));
    }

    private IEnumerator DoAfterDelayIEnumerator(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    private void CheckForInvSlotChange()
    {
        if (!InputSelectInventoryEnabled)
        {
        } else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _playerInventory.CurrentSlotIndex = 0;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _playerInventory.CurrentSlotIndex = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _playerInventory.CurrentSlotIndex = 2;
        } else if (_playerInventory.Slot(3) != null && Input.GetKeyDown(KeyCode.Alpha4))
        {
            _playerInventory.CurrentSlotIndex = 3;
        }
        else
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta > 0)
            {
                _playerInventory.DecrementSlotIndexIfPossible();
            } else if (scrollDelta < 0)
            {
                _playerInventory.IncrementSlotIndexIfPossible();
            }
        }
    }

}