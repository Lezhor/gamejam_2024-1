using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EntityController : MonoBehaviour
    {
        
        [SerializeField] private Transform spriteToFlip;
        [SerializeField] private Animator entityAnimator;
        private bool _animating;
        protected Animator Animator => entityAnimator;

        [Header("Entity Stats")]
        [SerializeField]
        private float speed = 500f;

        private bool _right = true;

        private Vector2 _movement;

        private bool _moving = false;

        public Vector2 MoveVector
        {
            get => _movement;

            set
            {
                _movement = value;
                if (_animating && _movement.magnitude > 0 != _moving)
                {
                    _moving = _movement.magnitude > 0;
                    entityAnimator.SetBool(Moving, _moving);
                }
                
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

        private static readonly int Moving = Animator.StringToHash("Moving");

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animating = entityAnimator != null;
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