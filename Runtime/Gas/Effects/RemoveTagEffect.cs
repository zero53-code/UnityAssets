using System;
using Zero53.GameplayTags;

namespace Zero53.Gas.Effects
{
    [Serializable]
    public class RemoveTagEffect : InstantEffect
    {
        public Tag removeTag;

        public RemoveTagEffect(Tag removeTag)
        {
            this.removeTag = removeTag;
        }

        protected internal override void Apply()
        {
            tags.Remove(removeTag);
        }
    }
}