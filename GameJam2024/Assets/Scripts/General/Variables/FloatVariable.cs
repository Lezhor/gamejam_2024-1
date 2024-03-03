using System;
using UnityEngine;

namespace General.Variables
{
    [CreateAssetMenu(menuName = "Variables/Float Variable", fileName = "New Float Variable")]
    public class FloatVariable : ScriptableObject
    {
        public event Action<float, float> OnValueChanged;
        
        [SerializeField] private float value; // Value which is set in the inspector

        // Actual stored value
        private float _value;
        
        public float Value
        {
            get => _value;
            set
            {
                float oldValue = _value;
                _value = value;
                this.value = _value;
                if (_value != oldValue)
                {
                    OnValueChanged?.Invoke(oldValue, _value);
                }
            }
        }

        private void Awake()
        {
            Value = value;
        }

        private void OnValidate()
        {
            if (value != Value)
            {
                Value = value;
            } 
        }

        /*
         * Whenever source changes it updates own value
         */
        public void Bind(FloatVariable source)
        {
            source.OnValueChanged += SetValueIfChanged;
            SetValueIfChanged(value, source.Value);
        }

        public void UnBind(FloatVariable source)
        {
            source.OnValueChanged -= SetValueIfChanged;
        }

        private void SetValueIfChanged(float oldValue, float newValue)
        {
            if (oldValue != newValue)
            {
                Value = newValue;
            }
        }
    }
}