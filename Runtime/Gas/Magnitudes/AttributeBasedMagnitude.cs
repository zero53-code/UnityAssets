using System;

namespace Zero53.Gas.Magnitudes
{
    [Serializable]

    public class AttributeBasedMagnitude : Magnitude
    {
        public float coefficient;
        public AttributeData attributeData;

        public override float value => baseValue + attributeData.value * coefficient;
    }
}