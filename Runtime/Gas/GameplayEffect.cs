using System;
using Zero53.GameplayTags;

namespace Zero53.Gas
{
    [Serializable]
    public abstract class GameplayEffect
    {
        public GameplayAbilitySystem abilitySystem { get; internal set; }
        public TagContainer tags => abilitySystem.tags;

        public TAttributeSet GetAttributeSet<TAttributeSet>()
            where TAttributeSet : GameplayAttributeSet
        {
            return abilitySystem.GetAttributeSet<TAttributeSet>();
        }

        protected internal abstract void Apply();

        protected internal virtual void Remove()
        {
        }
    }
}