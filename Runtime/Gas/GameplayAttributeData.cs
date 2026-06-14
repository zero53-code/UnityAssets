using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.Aggregators;

namespace Zero53.Gas
{
    /// <summary>
    /// 属性
    /// </summary>
    [Serializable]
    public sealed class GameplayAttributeData
    {
        [SerializeField, HorizontalGroup, OnValueChanged("OnBaseValueChanged")]
        internal float _baseValue;

        [SerializeField, HorizontalGroup, OnValueChanged("OnCurrentValueChanged")]
        internal float _currentValue;

        [SerializeReference] 
        public IAggregator aggregator = new DefaultAggregator();
        
        private List<Modifier> _modifiers = new();

        internal GameplayAttributeData()
        {
        }

        internal void InitInternal(GameplayAttributeSet attributeSet)
        {
            this.attributeSet = attributeSet;
        }

        #region API

        public GameplayAttributeSet attributeSet { get; private set; }

        public float baseValue
        {
            get => _baseValue;
            set
            {
                attributeSet.PreAttributeBaseChange(this, ref value);
                _baseValue = value;
                RecalculateCurrentValue();
            }
        }

        public float currentValue
        {
            get => _currentValue;
            internal set
            {
                attributeSet.PreAttributeChange(this, ref value);
                _currentValue = value;
            }
        }

        public float value => currentValue;

        #endregion

        #region Modifier API

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

        public Modifier Add(Magnitude magnitude)
        {
            var modifier = Modifier.Add(magnitude);
            AddModifier(modifier);
            return modifier;
        }

        public Modifier Multiply(Magnitude magnitude)
        {
            var modifier = Modifier.Multiply(magnitude);
            AddModifier(modifier);
            return modifier;
        }

        public Modifier Divide(Magnitude magnitude)
        {
            var modifier = Modifier.Divide(magnitude);
            AddModifier(modifier);
            return modifier;
        }

        public Modifier MultiplyCompound(Magnitude magnitude)
        {
            var modifier = Modifier.MultiplyCompound(magnitude);
            AddModifier(modifier);
            return modifier;
        }

        public Modifier AddFinal(Magnitude magnitude)
        {
            var modifier = Modifier.AddFinal(magnitude);
            AddModifier(modifier);
            return modifier;
        }

        public Modifier Override(Magnitude magnitude)
        {
            var modifier = Modifier.Override(magnitude);
            AddModifier(modifier);
            return modifier;
        }

        #endregion
        
        private void RecalculateCurrentValue()
        {
            currentValue = aggregator.Aggregate(_baseValue, _modifiers);
        }

        [Conditional("UNITY_EDITOR")]
        private void OnBaseValueChanged()
        {
            attributeSet?.PreAttributeBaseChange(this, ref _baseValue);
            RecalculateCurrentValue();
        }
        
        [Conditional("UNITY_EDITOR")]
        private void OnCurrentValueChanged()
        {
            RecalculateCurrentValue();
        }
    }
}