using System;
using Zero53.Utils;

namespace Zero53.Gas.Attributes.Processor
{
    [Serializable]
    public class  RangeChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name maxAttributeName;
        [NameDropdown] public Name minAttributeName;
        
        public void Process(GameplayAttribute attribute, ref float value)
        {
            var minValue = attribute.attributeSet[minAttributeName].value;
            var maxValue = attribute.attributeSet[maxAttributeName].value;
            value = value.Clamp(minValue, maxValue);
        }
    }
}