using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero53.Gas.AttributeSets
{
    /// <summary>
    /// 属性
    /// </summary>
    [Serializable]
    public sealed class AttributeData
    {
        [field: SerializeField, TableColumnWidth(width: 100)]
        public float baseValue { get; internal set; }

        [field: SerializeField, TableColumnWidth(width: 100)]
        public float currentValue { get; internal set; }

        public AttributeData(float baseValue) : this(baseValue, baseValue)
        {
        }

        public AttributeData(float baseValue, float initialValue)
        {
            this.baseValue = baseValue;
            this.currentValue = initialValue;
        }

        public float value
        {
            get => currentValue;
            set => currentValue = value;
        }
    }
}

