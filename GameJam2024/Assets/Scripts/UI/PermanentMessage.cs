using System;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshPro))]
    public class PermanentMessage : MonoBehaviour
    {

        public event Action<PermanentMessage> OnDestroyPeramanentMessage;
    
        private TextMeshPro _tmp;
        public Vector3 offset = new(0, 0.5f);
        public float fadeTime = 1f;

        private Vector2Int _startPos;

        public Vector2Int StartPos => _startPos;

        public bool FadeIn { get; set; } = true;
        
        public string Text
        {
            get => _tmp.text;
            set
            {
                _tmp.text = value;
                FadeIn = true;
            }
        }
        
        private void Awake()
        {
            Vector3 pos = transform.position;
            _startPos = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
            pos += offset;
            transform.position = pos;
            _tmp = GetComponent<TextMeshPro>();
            Text = "";
            _tmp.alpha = 0f;
        }
        
        private void Update()
        {
            if (FadeIn)
            {
                _tmp.alpha = Mathf.Min(1, _tmp.alpha + Time.deltaTime / fadeTime);
            }
            else
            {
                _tmp.alpha = Mathf.Max(0, _tmp.alpha - Time.deltaTime / fadeTime);
                if (_tmp.alpha == 0)
                {
                    OnDestroyPeramanentMessage?.Invoke(this);
                    Destroy(gameObject);
                }
            }
        }

    }
}
