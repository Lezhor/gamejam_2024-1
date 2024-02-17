using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshPro))]
    public class Message : MonoBehaviour
    {

        private TextMeshPro _tmp;
        public Color positiveColor = new(0, 1, 0, 1);
        public Color neutralColor = new(1, 1, 1, 1);
        public Color negativeColor = new(1, 0, 0, 1);
        public Vector3 offset = new(0, 0);
        
        [Header("Fade Out")] 
        public float fadeTime = 1f;
        public Vector3 velocity = new Vector2(0, 1f);

        public string Text
        {
            get => _tmp.text;
            set => _tmp.text = value;
        }

        private void Awake()
        {
            Debug.Log("Instantiated Message at position: " + transform.position);
            transform.position += offset;
            _tmp = GetComponent<TextMeshPro>();
            SetText("");
            _tmp.alpha = 1f;
        }

        private void Update()
        {
            transform.position += velocity * Time.deltaTime;

            _tmp.alpha = Mathf.Max(0, _tmp.alpha - Time.deltaTime / fadeTime);
            if (_tmp.alpha == 0)
                Destroy(gameObject);
        }

        public void SetText(string text, bool positive)
        {
            SetText(text, positive ? MessageType.Positive : MessageType.Negative);
        }

        public void SetText(string text)
        {
            SetText(text, MessageType.Neutral);
        }

        private void SetText(string text, MessageType type)
        {
            Text = text;
            _tmp.color = type switch
            {
                MessageType.Positive => positiveColor,
                MessageType.Neutral => neutralColor,
                MessageType.Negative => negativeColor,
                _ => neutralColor
            };

        }

        private enum MessageType
        {
            Positive,
            Negative,
            Neutral
        }
    }
}