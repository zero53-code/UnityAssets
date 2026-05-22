using System;

namespace Zero53.Gas.Attributes.Processor
{
    [Serializable]
    public class  AddChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name addendAttributeName;
        public void Process(GameplayAttributeSet attributeSet, Name attributeName, ref float value)
        {
            var addendValue = attributeSet[addendAttributeName];
            value += addendValue;
        }
    }
}