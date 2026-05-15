using System;
using Zero53.Utils;

namespace Zero53.Gas.Attributes.Processor
{
    [Serializable]
    public class  MaxChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name maxAttributeName;
        
        public void Process(GameplayAttribute attribute, ref float value)
        {
            var maxValue = attribute.attributeSet[maxAttributeName].value;
            value = value.Max(maxValue);
        }
    }
}