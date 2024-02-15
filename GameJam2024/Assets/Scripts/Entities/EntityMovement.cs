using System;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EntityMovement : MonoBehaviour
    {
        
        [SerializeField] private Transform spriteToFlip;

        [Header("Entity Stats")]
        [SerializeField]
        private float speed = 500f;

        private bool _right = true;

        private Vector2 _movement;

        public Vector2 MoveVector
        {
            get => _movement;

            set
            {
                _movement = value;
                if (_right && _movement.x < 0)
                {
                    _right = false;
                }
                else if (!_right && _movement.x > 0)
                {
                    _right = true;
                }
                else
                {
                    return;
                }

                UpdateSpriteScale(_right);
            }
        }
        
        [NonSerialized]
        protected Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void UpdateSpriteScale(bool right)
        {
            Vector3 localScale = spriteToFlip.localScale;
            localScale = new Vector3(
                Math.Abs(localScale.x) * (right ? 1 : -1),
                localScale.y,
                localScale.z
            );
            spriteToFlip.localScale = localScale;
        }

        private void FixedUpdate()
        {
            Move();
            FixedUpdateAddOn();
        }

        protected abstract void FixedUpdateAddOn();
        
        private void Move()
        {
            _rigidbody.AddForce(_movement * (speed * Time.fixedDeltaTime), ForceMode2D.Force);
        }
    }
}