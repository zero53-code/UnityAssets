using System;

namespace Zero53.Gas.AttributeSet.Processor
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