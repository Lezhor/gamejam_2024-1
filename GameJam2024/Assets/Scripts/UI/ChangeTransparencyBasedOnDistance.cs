using System;
using GameLogic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChangeTransparencyBasedOnDistance : MonoBehaviour
    {
        public Transform target;
        public float distance0 = 1.5f;
        public float distance1 = 5f;

        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (target == null)
            {
                target = GameManager.Instance.Player.transform;
            }
        }

        void Update()
        {
            Color temp = _renderer.color;
            float distance = (target.position - transform.position).magnitude;
            temp.a = Mathf.Clamp((distance - distance0) / (distance1 - distance0), 0f, 1f);
            _renderer.color = temp;
        }
    }
}
