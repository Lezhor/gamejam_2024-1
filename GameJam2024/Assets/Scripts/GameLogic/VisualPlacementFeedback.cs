using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using TileData = GameLogic.world.TileData;

namespace GameLogic
{
    public class VisualPlacementFeedback : MonoBehaviour
    {
        public Tilemap overlayTransparent;
        public Tilemap overlayOutlines;

        [Header("Fade Out Settings")] [SerializeField]
        private AnimationCurve fadeOutCurve; // Has to have length 1!

        [SerializeField] private float fadeOutTimeShort = 1f;
        [SerializeField] private float fadeOutTimeLong = 2f;

        [Header("Tiles")] [SerializeField] private TileBase squareTile;
        [SerializeField] private Color positiveColor = Color.green;
        [SerializeField] private Color negativeColor = Color.red;

        private readonly Dictionary<Vector2Int, Task> _fadeOutTasks = new();

        private void Awake()
        {
        }

        private void OnEnable()
        {
            GameManager.Instance.PlaceEvents.OnPlacementFailed += OnTilePlacementFailed;
        }

        private void OnDisable()
        {
            GameManager.Instance.PlaceEvents.OnPlacementFailed -= OnTilePlacementFailed;
        }

        private void Update()
        {
            List<Task> finishedTasks = new();

            foreach (KeyValuePair<Vector2Int, Task> entry in _fadeOutTasks)
            {
                Task task = entry.Value;
                if (Time.time >= task.StartTime + task.Duration)
                {
                    finishedTasks.Add(task);
                }
                else
                {
                    Redraw(task);
                }
            }

            foreach (Task task in finishedTasks)
            {
                EndTask(task.Pos);
            }
        }

        private void OnTilePlacementFailed(Vector2Int pos, TileData tile, List<Vector2Int> reasons)
        {
            StartTask(pos, tile, false, true, true);
            foreach (Vector2Int pos2 in reasons)
            {
                StartTask(pos2, false, true);
            }
        }

        private void StartTask(Vector2Int pos, bool positive, bool longFadeOut)
        {
            StartTask(pos, null, positive, false, longFadeOut);
        }

        private void StartTask(Vector2Int pos, TileData tile, bool positive, bool drawOutline, bool longFadeOut)
        {
            if (_fadeOutTasks.ContainsKey(pos))
            {
                EndTask(pos);
            }

            Task task = new Task(tile, pos, Time.time, longFadeOut ? fadeOutTimeLong : fadeOutTimeShort, positive,
                drawOutline);

            _fadeOutTasks.Add(task.Pos, task);

            float startValue = Mathf.Clamp(0, 1, fadeOutCurve.Evaluate(0));

            overlayTransparent.SetTile(task.Pos3D, squareTile);
            overlayTransparent.SetTileFlags(task.Pos3D, TileFlags.None);
            overlayTransparent.SetColor(task.Pos3D, task.Positive ? positiveColor : negativeColor);
            SetTransparency(overlayTransparent, task.Pos3D, startValue);
            if (drawOutline)
            {
                overlayOutlines.SetTile(task.Pos3D, task.Tile.wallsOutline);
                overlayOutlines.SetTileFlags(task.Pos3D, TileFlags.None);
                overlayOutlines.SetColor(task.Pos3D, task.Positive ? positiveColor : negativeColor);
                SetTransparency(overlayOutlines, task.Pos3D, startValue);
            }
        }

        private void Redraw(Task task)
        {
            float t = (Time.time - task.StartTime) / task.Duration;
            t = Mathf.Clamp(t, 0, 1);

            float value = Mathf.Clamp(fadeOutCurve.Evaluate(t), 0, 1);

            SetTransparency(overlayTransparent, task.Pos3D, value);
            if (task.DrawOutline)
            {
                SetTransparency(overlayOutlines, task.Pos3D, value);
            }
        }

        private void SetTransparency(Tilemap tilemap, Vector3Int pos, float value)
        {
            Color color = tilemap.GetColor(pos);
            color.a = value;
            tilemap.SetColor(pos, color);
        }

        private void EndTask(Vector2Int pos)
        {
            Vector3Int pos3D = new Vector3Int(pos.x, pos.y);
            overlayTransparent.SetTile(pos3D, null);
            overlayOutlines.SetTile(pos3D, null);
            _fadeOutTasks.Remove(pos);
        }

        private void OnValidate()
        {
            Keyframe lastKeyFrame = fadeOutCurve[fadeOutCurve.length - 1];
            if (lastKeyFrame.time < 1)
            {
                Debug.LogError("The length of the animation has to be at least 1 sec!");
                Keyframe newKeyFrame = new Keyframe(1, lastKeyFrame.value);
                fadeOutCurve.AddKey(newKeyFrame);
            }
        }

        private class Task
        {
            public readonly TileData Tile;
            public readonly Vector2Int Pos;
            public readonly float StartTime;
            public readonly float Duration;
            public readonly bool Positive;
            public readonly bool DrawOutline;

            public Vector3Int Pos3D => new(Pos.x, Pos.y);

            public Task(TileData tile, Vector2Int pos, float startTime, float duration, bool positive, bool drawOutline)
            {
                Tile = tile;
                Pos = pos;
                StartTime = startTime;
                Duration = duration;
                Positive = positive;
                DrawOutline = drawOutline;
            }

            public override bool Equals(object obj)
            {
                Task that = obj as Task;
                return that != null && this.Pos.Equals(that.Pos);
            }

            public override int GetHashCode()
            {
                return Pos.GetHashCode();
            }
        }
    }
}