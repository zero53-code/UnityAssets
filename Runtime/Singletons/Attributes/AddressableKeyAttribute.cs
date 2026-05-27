using System;

namespace Zero53.Singletons.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AddressableKeyAttribute : Attribute
    {
        public string key;
        public AddressableKeyAttribute(string key = null)
        {
            this.key = key;
        }
    }
}