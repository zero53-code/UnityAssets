using System;

namespace Zero53.Gas.Attributes.Processor
{
    [Serializable]
    public class  AddChangeProcessor : IChangeProcessor
    {
        [NameDropdown] public Name addendAttributeName;
        public void Process(GameplayAttribute attribute, ref float value)
        {
            var addendValue = attribute.attributeSet[addendAttributeName].value;
            value += addendValue;
        }
    }
}