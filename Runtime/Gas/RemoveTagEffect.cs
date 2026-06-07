using System;
using Zero53.GameplayTags;

namespace Zero53.Gas
{
    [Serializable]
    public class RemoveTagEffect : InstantEffect
    {
        public Tag removeTag;

        public RemoveTagEffect(Tag removeTag)
        {
            this.removeTag = removeTag;
        }

        protected internal override void Update(float deltaTime)
        {
            tags.Remove(removeTag);
        }
    }
}