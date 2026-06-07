using System;

namespace Zero53.Gas.Magnitudes
{
    [Serializable]
    public class BaseValueMagnitude : Magnitude
    {
        public override float value => baseValue;
    }
}