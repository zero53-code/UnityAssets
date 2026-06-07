using System;
using Zero53.GameplayTags;

namespace Zero53.Gas.Effects
{
    [Serializable]
    public class AddTagEffect : InstantEffect
    {
        public Tag addTag;

        public AddTagEffect(Tag addTag)
        {
            this.addTag = addTag;
        }

        protected internal override void Update(float deltaTime)
        {
            tags.Add(addTag);
        }
    }
}