using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        [Header("Tiles")] 
        [SerializeField] private TileBase squareTile;
        [SerializeField] private Color positiveColor = Color.green;
        [SerializeField] private Color negativeColor = Color.red;

        private Dictionary<Vector3Int, Task> _fadeOutTasks = new();

        private void Update()
        {
            List<Task> finishedTasks = new();
            
            foreach (KeyValuePair<Vector3Int, Task> entry in _fadeOutTasks)
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

        private void StartTask(WorldTile tile, bool positive, bool drawOutline, bool longFadeOut)
        {
            if (_fadeOutTasks.ContainsKey(tile.Pos))
            {
                EndTask(tile.Pos);
            }

            Task task = new Task(tile, Time.time, longFadeOut ? fadeOutTimeLong : fadeOutTimeShort, positive,
                drawOutline);
            overlayTransparent.SetTile(task.Pos, squareTile);
            overlayTransparent.SetColor(task.Pos, positive ? positiveColor : negativeColor);
            SetTransparency(overlayTransparent, task.Pos, 1);
            if (drawOutline)
            {
                overlayOutlines.SetTile(task.Pos, task.Tile.Data.wallsOutline);
                SetTransparency(overlayOutlines, task.Pos, 1);
            }
        }

        private void Redraw(Task task)
        {
            float t = (Time.time - task.StartTime) / task.Duration;
            t = Mathf.Clamp(t, 0, 1);

            float value = Mathf.Clamp(fadeOutCurve.Evaluate(t), 0, 1);

            SetTransparency(overlayTransparent, task.Pos, value);
            if (task.DrawOutline)
            {
                SetTransparency(overlayOutlines, task.Pos, value);
            }
        }

        private void SetTransparency(Tilemap tilemap, Vector3Int pos, float value)
        {
            Color color = tilemap.GetColor(pos);
            color.a = value;
            tilemap.SetColor(pos, color);
        }

        private void EndTask(Vector3Int pos)
        {
            overlayTransparent.SetTile(pos, null);
            overlayOutlines.SetTile(pos, null);
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

        public class Task
        {
            public readonly WorldTile Tile;
            public readonly float StartTime;
            public readonly float Duration;
            public readonly bool Positive;
            public readonly bool DrawOutline;

            public Vector3Int Pos => Tile.Pos;

            public Task(WorldTile tile, float startTime, float duration, bool positive, bool drawOutline)
            {
                Tile = tile;
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