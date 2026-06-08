using System;
using Zero53.GameplayTags;

namespace Zero53.Gas.GameplayEffects
{
    [Serializable]
    public class AddTagEffect : InstantGameplayEffect
    {
        public Tag addTag;

        public AddTagEffect(Tag addTag)
        {
            this.addTag = addTag;
        }

        protected internal override void Apply()
        {
            tags.Add(addTag);
        }
    }
}