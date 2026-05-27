using System;
using Zero53.Utils;

namespace Zero53.Gas.AttributeSet.Processor
{
    [Serializable]
    public class MinChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name minAttributeName;
        
        public void Process(GameplayAttributeSet attributeSet, Name attributeName, ref float value)
        {
            var minValue = attributeSet[minAttributeName];
            value = value.Min(minValue);
        }
    }
}