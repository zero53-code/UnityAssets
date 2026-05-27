using System;
using Zero53.Utils;

namespace Zero53.Gas.AttributeSet.Processor
{
    [Serializable]
    public class  RangeChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name maxAttributeName;
        [NameDropdown] public Name minAttributeName;
        
        public void Process(GameplayAttributeSet attributeSet, Name attributeName, ref float value)
        {
            var minValue = attributeSet[minAttributeName];
            var maxValue = attributeSet[maxAttributeName];
            value = value.Clamp(minValue, maxValue);
        }
    }
}