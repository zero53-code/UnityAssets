using System;
using Zero53.Utils;

namespace Zero53.Gas.Attributes.Processor
{
    [Serializable]
    public class MinChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name minAttributeName;
        
        public void Process(GameplayAttribute attribute, ref float value)
        {
            var minValue = attribute.attributeSet[minAttributeName].value;
            value = value.Min(minValue);
        }
    }
}