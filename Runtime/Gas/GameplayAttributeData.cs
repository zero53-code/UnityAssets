using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.Magnitudes;

namespace Zero53.Gas
{
    /// <summary>
    /// 属性
    /// </summary>
    [Serializable]
    public sealed class GameplayAttributeData
    {
        [SerializeField, TableColumnWidth(width: 100)]
        internal float _baseValue;

        [SerializeField, TableColumnWidth(width: 100)]
        internal float _currentValue;

        [SerializeReference, HideInInspector] 
        public IAggregator aggregator = new DefaultAggregator();
        
        internal List<Modifier> modifiers = new();

        internal GameplayAttributeData()
        {
        }

        internal void Init(GameplayAttributeSet attributeSet)
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
                currentValue = aggregator.Aggregate(_baseValue, modifiers);
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

        public GameplayAttributeSet attributeSet { get; private set; }

        public float value
        {
            get => currentValue;
            set => currentValue = value;
        }
    }
}

