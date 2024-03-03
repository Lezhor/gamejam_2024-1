using General.Variables;
using UnityEngine;

namespace temp
{
    public class TestVariables : MonoBehaviour
    {
        public FloatReference health;
        public FloatReference damage;

        private void OnEnable()
        {
            if (damage.Variable != null)
            {
                damage.Variable.OnValueChanged += (oldValue, newValue) =>
                    Debug.Log("Changed from " + oldValue + " to " + newValue);
            }
        }

        private void Update()
        {
            Debug.Log("Health: " + health.Value);
            Debug.Log("Damage: " + damage.Value);

            if (Input.GetKeyDown(KeyCode.A))
            {
                health.Value = 10f;
            }
            /*
            if (Input.GetKeyDown(KeyCode.B))
            {
                damage.Variable.Bind(health.Variable);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                damage.Variable.UnBind(health.Variable);
            }
            */
        }
    }
}