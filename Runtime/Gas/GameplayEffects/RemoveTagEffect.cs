using System;
using Zero53.GameplayTags;

namespace Zero53.Gas.GameplayEffects
{
    [Serializable]
    public class RemoveTagEffect : InstantGameplayEffect
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