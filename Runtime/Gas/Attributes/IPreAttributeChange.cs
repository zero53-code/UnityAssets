using System;
using Zero53.Utils;

namespace Zero53
{
    public interface IPreAttributeChange
    {
        void OnPreChange(GameplayAttribute attribute, ref float value);
    }

    [Serializable]
    public struct MaxPreAttributeChange : IPreAttributeChange
    {
        [NameDropdown] public Name maxAttributeName;
        
        public void OnPreChange(GameplayAttribute attribute, ref float value)
        {
            var maxValue = attribute.attributeSet[maxAttributeName].value;
            value = value.Max(maxValue);
        }
    }
    
    [Serializable]
    public struct MinPreAttributeChange : IPreAttributeChange
    {
        [NameDropdown] public Name minAttributeName;
        
        public void OnPreChange(GameplayAttribute attribute, ref float value)
        {
            var minValue = attribute.attributeSet[minAttributeName].value;
            value = value.Min(minValue);
        }
    }
    
    [Serializable]
    public struct RangePreAttributeChange : IPreAttributeChange
    {
        [NameDropdown] public Name maxAttributeName;
        [NameDropdown] public Name minAttributeName;
        
        public void OnPreChange(GameplayAttribute attribute, ref float value)
        {
            var minValue = attribute.attributeSet[minAttributeName].value;
            var maxValue = attribute.attributeSet[maxAttributeName].value;
            value = value.Clamp(minValue, maxValue);
        }
    }

    [Serializable]
    public struct MultiplyPreAttributeChange : IPreAttributeChange
    {
        [NameDropdown] public Name multipleAttributeName;
        public void OnPreChange(GameplayAttribute attribute, ref float value)
        {
            var multipleValue = attribute.attributeSet[multipleAttributeName].value;
            value *= multipleValue;
        }
    }

    [Serializable]
    public struct AddPreAttributeChange : IPreAttributeChange
    {
        [NameDropdown] public Name addendAttributeName;
        public void OnPreChange(GameplayAttribute attribute, ref float value)
        {
            var addendValue = attribute.attributeSet[addendAttributeName].value;
            value += addendValue;
        }
    }
}