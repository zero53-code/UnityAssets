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
        [SerializeField, TableColumnWidth(width: 100)] 
         internal float _baseValue;
        [SerializeField, TableColumnWidth(width: 100)] 
         internal float _currentValue;
        
        internal AttributeData() { }

        internal void Init(AttributeSet attributeSet)
        {
            this.attributeSet = attributeSet;
        }
        
        public float baseValue
        {
            get => _baseValue;
            set
            {
                attributeSet.PreAttributeBaseChange(this, ref value);
                _baseValue = value;
            }
        }

        public float currentValue
        {
            get => _currentValue;
            set
            {
                attributeSet.PreAttributeChange(this, ref value);
                _currentValue = value;
            }
        }

        public AttributeSet attributeSet { get; private set; }

        public float value
        {
            get => currentValue;
            set => currentValue = value;
        }
    }
}

