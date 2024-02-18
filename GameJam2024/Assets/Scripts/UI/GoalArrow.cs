using System;
using GameLogic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GoalArrow : MonoBehaviour
    {
        [SerializeField] private float posOnViewPort = .9f;
        [SerializeField] private float distanceToConsiderTargetAsReached = 1.5f;
        private Vector3 _target;
        private Camera _camera;

        private SpriteRenderer _renderer;

        public void Start()
        {
            _target = transform.position;

            if (_camera == null)
                _camera = GameManager.Instance.Cam;

            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            Vector3 viewportTarget = _camera.WorldToViewportPoint(_target);

            bool targetVisible = viewportTarget.x is >= 0 and <= 1
                                 && viewportTarget.y is >= 0 and <= 1;

            Vector3 direction = new();
            if (!targetVisible)
            {

                Vector3 viewportCam = new Vector3(0.5f, 0.5f);

                Vector3 viewportDistance = viewportTarget - viewportCam;
                Vector3 newViewportDistance = Mathf.Abs(viewportDistance.x) > Mathf.Abs(viewportDistance.y)
                    ? viewportDistance / (Mathf.Abs(viewportDistance.x) * 2f)
                    : viewportDistance / (Mathf.Abs(viewportDistance.y) * 2f);

                newViewportDistance *= posOnViewPort;

                Vector3 newViewportPos = viewportCam + newViewportDistance;

                Vector3 finalWorldPos = _camera.ViewportToWorldPoint(newViewportPos);
                finalWorldPos.z = 0;

                transform.position = finalWorldPos;

                direction = _target - finalWorldPos;

                targetVisible = direction.magnitude < distanceToConsiderTargetAsReached;

            }

            if (!targetVisible)
            {
                _renderer.enabled = true;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                _renderer.enabled = false;
            }
        }
    }
}