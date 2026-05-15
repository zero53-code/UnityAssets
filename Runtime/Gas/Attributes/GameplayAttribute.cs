using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero53
{
    [Serializable]
    public class GameplayAttribute
    {
        internal List<IPreAttributeChange> preAttributeChangeList;
        [field: SerializeField, HorizontalGroup, LabelText("base")] 
        public float baseValue {get; private set;}
        [field: SerializeField, HorizontalGroup, LabelText("current")] 
        public float currentValue {get; private set;}

        internal GameplayAttribute(GameplayAttributeSet attributeSet, float baseValue)
        {
            preAttributeChangeList = new List<IPreAttributeChange>();
            this.attributeSet = attributeSet;
            this.baseValue = baseValue;
            this.currentValue = baseValue;
        }
        
        public GameplayAttributeSet attributeSet { get; }

        public float value 
        {
            get => currentValue;
            set => SetValue(value);
        }

        public void SetValue(float newValue)
        {
            foreach (var attributePostProcessor in preAttributeChangeList)
            {
                attributePostProcessor.OnPreChange(this, ref newValue);
            }
            currentValue = newValue;
        }
    }
}
