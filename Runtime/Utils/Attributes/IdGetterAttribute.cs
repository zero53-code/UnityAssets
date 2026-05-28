using System;

namespace Zero53.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class IdGetterAttribute : Attribute
    {
        public string name;
        public IdGetterAttribute(string name = "id")
        {
            this.name = name;
        }
    }
}