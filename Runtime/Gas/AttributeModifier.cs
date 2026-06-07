using System;

namespace Zero53.Gas
{
    [Serializable]
    public struct AttributeModifier
    {
        public AttributeModifierOp op;
        public float value;

        private AttributeModifier(AttributeModifierOp op, float value)
        {
            this.op = op;
            this.value = value;
        }
        
        public static AttributeModifier Add(float value) => new(AttributeModifierOp.Add, value);
        public static AttributeModifier Multiple(float value) => new(AttributeModifierOp.Multiple, value);
        public static AttributeModifier Divide(float value) => new(AttributeModifierOp.Divide, value);
        public static AttributeModifier Override(float value) => new(AttributeModifierOp.Override, value);
    }
}