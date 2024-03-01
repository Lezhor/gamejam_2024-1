using System;
using GameLogic.world;
using UnityEngine;

namespace GameLogic.player
{
    public class PlayerInventory
    {
        public event Action<int, TileData> OnSlotContentChanged;
        public event Action<int> OnActiveSlotChanged;
        public event Action<TileData> OnActiveTileChanged; // When a new slot is picked unless its the same tile-type
        public event Action<int, int> OnGoldValueChanged;

        public event Action OnGoldRanOut;


        private int gold = 0;

        public int Gold
        {
            get => gold;
            set
            {
                int oldValue = gold;
                gold = value;
                OnGoldValueChanged?.Invoke(oldValue, value);
                if (gold <= 0)
                {
                    OnGoldRanOut?.Invoke();
                }
            }
        }

        private readonly TileData[] _slots = new TileData[4];

        private int _selectedSlot = 0;

        public TileData Slot(int index) => _slots[index];

        public void SetSpecialSlot(TileData tile)
        {
            _slots[3] = tile;
            OnSlotContentChanged?.Invoke(3, tile);
            CurrentSlotIndex = 3;
        }

        public TileData CurrentSlot
        {
            get => _slots[_selectedSlot];
            set
            {
                _slots[_selectedSlot] = value;
                OnSlotContentChanged?.Invoke(_selectedSlot, value);
                OnActiveTileChanged?.Invoke(value);
            }
        }

        public void ReplaceCurrentSlot()
        {
            if (_selectedSlot == 3)
            {
                CurrentSlot = null;
                CurrentSlotIndex = 2;
            }
            else
            {
                CurrentSlot = _randomizer.GetRandomTile();
            }
        }

        public int CurrentSlotIndex
        {
            get => _selectedSlot;
            set
            {
                if (value >= 0 && value <= 3)
                {
                    if (_selectedSlot != value)
                    {
                        int oldSlot = _selectedSlot;
                        _selectedSlot = value;
                        OnActiveSlotChanged?.Invoke(value);
                        if (Slot(oldSlot) != Slot(_selectedSlot))
                        {
                            OnActiveTileChanged?.Invoke(Slot(_selectedSlot));
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Tried to set _selectedSlot to " + value);
                }
            }
        }

        public void IncrementSlotIndexIfPossible() =>
            CurrentSlotIndex = Math.Min(CurrentSlotIndex + 1, _slots[3] != null ? 3 : 2);

        public void DecrementSlotIndexIfPossible() => CurrentSlotIndex = Math.Max(CurrentSlotIndex - 1, 0);


        private TileRandomizer _randomizer;

        public PlayerInventory(TileRandomizer randomizer) : this(randomizer,
            randomizer.GetRandomTile(), randomizer.GetRandomTile(),
            randomizer.GetRandomTile(), null)
        {
        }

        private PlayerInventory(TileRandomizer randomizer,
            TileData startSlot1, TileData startSlot2,
            TileData startSlot3, TileData startSlot4)
        {
            _randomizer = randomizer;
            _selectedSlot = 0;
            CurrentSlot = startSlot1;
            _selectedSlot = 1;
            CurrentSlot = startSlot2;
            _selectedSlot = 2;
            CurrentSlot = startSlot3;
            _selectedSlot = 3;
            CurrentSlot = startSlot4;
            CurrentSlotIndex = 0;
        }
    }
}