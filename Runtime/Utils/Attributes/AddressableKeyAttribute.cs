using System;

namespace Zero53.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class AddressableKeyAttribute : Attribute
    {
        public string[] keys;

        public AddressableKeyAttribute(params string[] keys)
        {
            this.keys = keys;
        }
        
        public AddressableKeyAttribute(string key = null)
        {
            this.keys = new[] { key };
        }
    }
}