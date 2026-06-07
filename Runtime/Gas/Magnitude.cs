using System;

namespace Zero53.Gas
{
    [Serializable]
    public abstract class Magnitude
    {
        public float baseValue;
        public abstract float value { get; }

        public static implicit operator float(Magnitude magnitude)
        {
            return magnitude.value;
        }
    }
}