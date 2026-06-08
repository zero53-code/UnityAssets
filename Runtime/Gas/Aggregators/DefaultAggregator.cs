using System;
using System.Collections.Generic;

namespace Zero53.Gas.Aggregators
{
    /// <summary>
    /// 默认聚合器 
    /// </summary>
    public class DefaultAggregator : IAggregator
    {
        public float Aggregate(float baseValue, IList<Modifier> modifiers)
        {
            var additive = 0f;
            var multiplicative = 1f;
            var division = 1f;
            var overrideValue = baseValue;
            var multiplicativeCompound = 1f;
            var additiveFinal = 0f;

            foreach (var modifier in modifiers)
            {
                var magnitude = (float)modifier.magnitude;
                
                switch (modifier.op)
                {
                    case ModifierOp.Add:
                        additive += magnitude;
                        break;
                    case ModifierOp.Multiply:
                        multiplicative += magnitude - 1;
                        break;
                    case ModifierOp.Divide:
                        division += magnitude - 1;
                        break;
                    case ModifierOp.Override:
                        overrideValue = magnitude;
                        additive = 0;
                        multiplicative = 1;
                        multiplicativeCompound = 1;
                        additiveFinal = 0;
                        break;
                    case ModifierOp.MultiplyCompound:
                        multiplicativeCompound *= magnitude;
                        break;
                    case ModifierOp.AddFinal:
                        additiveFinal += magnitude;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(modifiers));
                }
            }

            return (overrideValue * multiplicativeCompound + additive) * multiplicative / division + additiveFinal;
        }
    }
}