using System;

namespace Zero53.GameplayTags.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class ExcludeGameplayTagAttribute : Attribute
    {
        public string[] tags;
        public bool isExact;

        public ExcludeGameplayTagAttribute(params string[] tags)
        {
            this.tags = tags;
        }

        public ExcludeGameplayTagAttribute(string tag) : this(new[] { tag })
        {
        }
    }
}