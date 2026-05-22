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
    internal class GameplayAttributeData
    {
        [SerializeReference, ReadOnly, HideInInspector]
        public List<IChangeProcessor> changeProcessors;

        [TableColumnWidth(width: 50)]
        public Name name;

        [TableColumnWidth(width: 100)]
        public float baseValue;

        [TableColumnWidth(width: 100)]
        public float currentValue;

        public GameplayAttributeData(Name name, float baseValue) : this(name, baseValue, baseValue)
        {
        }
        
        public GameplayAttributeData(Name name, float baseValue, float initialValue)
        {
            changeProcessors = new List<IChangeProcessor>();
            this.name = name;
            this.baseValue = baseValue;
            this.currentValue = initialValue;
        }
        
        public float value => currentValue;

        public void SetValue(GameplayAttributeSet attributeSet, float newValue)
        {
            foreach (var attributePostProcessor in changeProcessors)
            {
                attributePostProcessor.Process(attributeSet, name, ref newValue);
            }
            currentValue = newValue;
        }
    }
}
