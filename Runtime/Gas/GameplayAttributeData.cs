using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.Aggregators;
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
        
        private List<Modifier> _modifiers = new();

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
                attributeSet?.PreAttributeBaseChange(this, ref value);
                _baseValue = value;
                RecalculateCurrentValue();
            }
        }

        public float currentValue
        {
            get => _currentValue;
            set
            {
                attributeSet?.PreAttributeChange(this, ref value);
                _currentValue = value;
            }
        }

        public GameplayAttributeSet attributeSet { get; private set; }

        public float value
        {
            get => currentValue;
            set => currentValue = value;
        }

        public void AddModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
            RecalculateCurrentValue();
        }

        public bool RemoveModifier(Modifier modifier)
        {
            if (!_modifiers.Remove(modifier)) return false;
            
            RecalculateCurrentValue();
            return true;
        }

        private void RecalculateCurrentValue()
        {
            currentValue = aggregator.Aggregate(_baseValue, _modifiers);
        }
    }
}

