using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.Attributes.Processor;

namespace Zero53.Gas.Attributes
{
    /// <summary>
    /// 属性
    /// </summary>
    [Serializable]
    public class GameplayAttribute
    {
        public event Action<GameplayAttribute, float> OnPreChange; 
        
        internal List<IChangeProcessor> changeProcessors;
        
        [field: SerializeField, HorizontalGroup, LabelText("base")] 
        public float baseValue {get; private set;}
        
        [field: SerializeField, HorizontalGroup, LabelText("current")] 
        public float currentValue {get; private set;}
        
        public GameplayAttributeSet attributeSet { get; }

        internal GameplayAttribute(GameplayAttributeSet attributeSet, float baseValue)
        {
            changeProcessors = new List<IChangeProcessor>();
            this.attributeSet = attributeSet;
            this.baseValue = baseValue;
            this.currentValue = baseValue;
        }
        
        public float value 
        {
            get => currentValue;
            set => SetValue(value);
        }

        public void SetValue(float newValue)
        {
            OnPreChange?.Invoke(this, newValue);
            
            foreach (var attributePostProcessor in changeProcessors)
            {
                attributePostProcessor.Process(this, ref newValue);
            }
            currentValue = newValue;
        }
    }
}
