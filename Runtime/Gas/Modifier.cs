using System;
using UnityEngine;

namespace Zero53.Gas
{
    [Serializable]
    public struct Modifier
    {
        public ModifierOp op;
        [SerializeReference] public Magnitude magnitude;

        private Modifier(ModifierOp op, Magnitude magnitude)
        {
            this.op = op;
            this.magnitude = magnitude;
        }
        
        public static Modifier Add(Magnitude magnitude) => new(ModifierOp.Add, magnitude);
        public static Modifier Multiply(Magnitude magnitude) => new(ModifierOp.Multiply, magnitude);
        public static Modifier Divide(Magnitude magnitude) => new(ModifierOp.Divide, magnitude);
        public static Modifier MultiplyCompound(Magnitude magnitude) => new(ModifierOp.MultiplyCompound, magnitude);
        public static Modifier Override(Magnitude magnitude) => new(ModifierOp.Override, magnitude);
    }
}