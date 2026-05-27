using System;
using Zero53.Utils;

namespace Zero53.Gas.AttributeSet.Processor
{
    [Serializable]
    public class  MaxChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name maxAttributeName;
        
        public void Process(GameplayAttributeSet attributeSet, Name attributeName, ref float value)
        {
            var maxValue = attributeSet[maxAttributeName];
            value = value.Max(maxValue);
        }
    }
}