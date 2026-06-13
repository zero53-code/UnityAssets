using System;

namespace Zero53.GameplayTags.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | 
                    AttributeTargets.Enum | AttributeTargets.Interface, Inherited = false)]
    public sealed class GameplayTagBaseAttribute : Attribute
    {
        public string tag;

        public GameplayTagBaseAttribute(string tag)
        {
            this.tag = tag;
        }
    }
}