using System;
using Zero53.GameplayTags;
using Zero53.Gas.AttributeSets;

namespace Zero53.Gas.Effects
{
    [Serializable]
    public abstract class GameplayEffect
    {
        public AbilitySystem abilitySystem { get; internal set; }
        public TagContainer tags => abilitySystem.tags;

        public TAttributeSet GetAttributeSet<TAttributeSet>()
            where TAttributeSet : AttributeSet
        {
            return abilitySystem.GetAttributeSet<TAttributeSet>();
        }
        
        internal void InitInternal(AbilitySystem abilitySystem)
        {
            this.abilitySystem = abilitySystem;
            Init();
        }

        protected virtual void Init()
        {
        }

        protected internal abstract void Update(float deltaTime);
        
        protected internal abstract void Apply();
    }
}