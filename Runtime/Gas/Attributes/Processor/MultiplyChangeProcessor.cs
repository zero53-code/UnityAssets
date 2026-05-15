using System;

namespace Zero53.Gas.Attributes.Processor
{
    [Serializable]
    public class  MultiplyChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name multipleAttributeName;
        public void Process(GameplayAttribute attribute, ref float value)
        {
            var multipleValue = attribute.attributeSet[multipleAttributeName].value;
            value *= multipleValue;
        }
    }
}