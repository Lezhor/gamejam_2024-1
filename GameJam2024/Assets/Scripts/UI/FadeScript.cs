using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FadeScript : MonoBehaviour
    {
        public event Action OnFadeInStarted; 
        public event Action OnFadeInFinished; 
        
        [Header("Fade Stats")] 
        public float fadeDelay = 0f;
        public float fadeTime = 1f;
        [Header("Fade Children too?")]
        [SerializeField]
        private bool foreachChildImage = false;
        [SerializeField]
        private bool foreachChildTextMeshPro = false;

        private float _delayTimer;

        private float _fadeValue = 0f;

        private bool _fadingIn = true;

        private List<Image> _images;
        private List<TMP_Text> _tmps;

        private void OnEnable()
        {
            _fadeValue = 0f;
            _fadingIn = true;
            _delayTimer = fadeDelay;
            _images = foreachChildImage ? GetComponentsInChildren<Image>().ToList() : GetComponents<Image>().ToList();
            _tmps = foreachChildTextMeshPro ? GetComponentsInChildren<TMP_Text>().ToList() : GetComponents<TMP_Text>().ToList();
            UpdateAllAlphaValues(_fadeValue);
        }

        private void Update()
        {
            if (_delayTimer > 0)
            {
                _delayTimer -= Time.deltaTime;
                if (_delayTimer <= 0)
                {
                    OnFadeInStarted?.Invoke();
                }
            }
            else
            {
                float backup = _fadeValue;
                _fadeValue = Mathf.Clamp(
                    _fadeValue + (Time.deltaTime / fadeTime) * (_fadingIn ? 1 : -1),
                    0, 1);

                if (backup != _fadeValue)
                {
                    UpdateAllAlphaValues(_fadeValue);
                    if (backup != 1 && _fadeValue == 1)
                    {
                        OnFadeInFinished?.Invoke();
                    }
                }
            }
        }

        private void UpdateAllAlphaValues(float alpha)
        {
            _images.ForEach(image =>
            {
                Color color = image.color;
                color.a = alpha;
                image.color = color;
            });
            _tmps.ForEach(tmp =>
            {
                Color color = tmp.color;
                color.a = alpha;
                tmp.color = color;
            });
        }
    }
}