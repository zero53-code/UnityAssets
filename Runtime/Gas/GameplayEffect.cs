using UnityEngine;
using Zero53.GameplayTags;

namespace Zero53.Gas
{
    public abstract class GameplayEffect : ScriptableObject
    {
        public GameplayAbilitySystem abilitySystem { get; private set; }
        
        public TagContainer tags => abilitySystem.tags;

        public TAttributeSet GetAttributeSet<TAttributeSet>()
            where TAttributeSet : GameplayAttributeSet
        {
            return abilitySystem.GetAttributeSet<TAttributeSet>();
        }

        internal void InitInternal(GameplayAbilitySystem abilitySystem)
        {
            this.abilitySystem = abilitySystem;
        }

        protected internal abstract void OnApply();

        protected internal virtual void OnRemove()
        {
        }
    }
}