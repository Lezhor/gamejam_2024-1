using System;
using GameLogic;
using UnityEngine;

namespace Entities.Player
{
    public class TrackingPoint : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] [Range(0f, 1f)] private float moveFreedom = 0.5f;

        private Camera _cam;

        public bool Changing { get; set; } = true;

        private void Start()
        {
            _cam = GameManager.Instance.Cam;
        }

        public void FixedUpdate()
        {
            if (Changing)
            {
                Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 playerPos = player.transform.position;
                transform.localPosition = (mousePos - playerPos) * moveFreedom;
            }
        }
    }
}