using System;

namespace Zero53.Gas
{
    [Serializable]
    public struct AttributeModifier
    {
        public AttributeModifierOp op;
        public Magnitude magnitude;

        private AttributeModifier(AttributeModifierOp op, Magnitude magnitude)
        {
            this.op = op;
            this.magnitude = magnitude;
        }
        
        public static AttributeModifier Add(Magnitude magnitude) => new(AttributeModifierOp.Add, magnitude);
        public static AttributeModifier Multiply(Magnitude magnitude) => new(AttributeModifierOp.Multiply, magnitude);
        public static AttributeModifier Divide(Magnitude magnitude) => new(AttributeModifierOp.Divide, magnitude);
        public static AttributeModifier MultiplyCompound(Magnitude magnitude) => new(AttributeModifierOp.MultiplyCompound, magnitude);
        public static AttributeModifier Override(Magnitude magnitude) => new(AttributeModifierOp.Override, magnitude);
    }
}