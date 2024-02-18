using UnityEngine;

namespace UI
{
    public class PermanentRotation : MonoBehaviour
    {
        public float speed = .1f;

        private void Update()
        {
            transform.Rotate(Vector3.forward, 360 * speed * Time.deltaTime);
        }
    }
}
