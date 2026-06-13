using System;

namespace Zero53.GameplayTags.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class GameplayTagAttribute : Attribute
    {
        public string[] tags;
        public bool isExact;

        public GameplayTagAttribute(params string[] tags)
        {
            this.tags = tags;
        }

        public GameplayTagAttribute(string tag) : this(new[] { tag })
        {
        }
    }
}