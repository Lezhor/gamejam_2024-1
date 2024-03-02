using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace General
{
    public class CustomMath
    {
        public static T PickRandomOption<T>(List<T> options, Func<T, float> valueFunc)
        {
            float totalValue = options.Select(valueFunc).Sum();

            float pickedValue = Random.Range(0, totalValue);

            float current = 0f;

            foreach (T option in options)
            {
                current += valueFunc.Invoke(option);
                if (current > pickedValue)
                {
                    return option;
                }
            }

            return options[^1];
        }
    }
}