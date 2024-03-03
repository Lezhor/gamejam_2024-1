using System;

namespace General.Variables
{
    [Serializable]
    public class FloatReference
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public FloatVariable Variable;

            
        public float Value
        {
            get => UseConstant ? ConstantValue : Variable.Value;
            set
            {
                if (UseConstant)
                {
                    ConstantValue = value;
                }
                else
                {
                    Variable.Value = value;
                }
            }
        }

        public FloatReference()
        { }

        public FloatReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }
        
        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}