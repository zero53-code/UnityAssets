using System;

namespace Zero53.Gas.Attributes.Processor
{
    [Serializable]
    public class  MultiplyChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name multipleAttributeName;
        public void Process(GameplayAttributeSet attributeSet, Name attributeName, ref float value)
        {
            var multipleValue = attributeSet[multipleAttributeName];
            value *= multipleValue;
        }
    }
}