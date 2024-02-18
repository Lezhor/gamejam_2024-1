using System;
using GameLogic;
using UnityEngine;

namespace Entities.Player
{
    public class TrackingPoint : MonoBehaviour
    {
        [SerializeField] private Transform player;
        private PlayerController _playerScript;
        [SerializeField] [Range(0f, 1f)] private float moveFreedom = 0.5f;

        private Camera _cam;

        private void Start()
        {
            _cam = GameManager.Instance.Cam;
            _playerScript = GameManager.Instance.PlayerScript;
        }

        public void FixedUpdate()
        {
            if (_playerScript.InputMouseMoveEnabled)
            {
                Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 playerPos = player.transform.position;
                transform.localPosition = (mousePos - playerPos) * moveFreedom;
            }
        }
    }
}