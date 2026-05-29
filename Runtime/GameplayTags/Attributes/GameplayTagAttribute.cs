using System;

namespace Zero53.GameplayTags.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class GameplayTagAttribute  : Attribute
    {
        public string[] tags;

        public GameplayTagAttribute(params string[] tags)
        {
            this.tags = tags;
        }

        public GameplayTagAttribute(string tag) : this(new[] { tag })
        {
        }
    }
}