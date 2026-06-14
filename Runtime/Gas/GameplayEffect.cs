using UnityEngine;
using Zero53.GameplayTags;

namespace Zero53.Gas
{
    public abstract class GameplayEffect : ScriptableObject
    {
        public GameplayAbilitySystem owner { get; private set; }
        
        public TagContainer tags => owner.tags;

        public TAttributeSet GetAttributeSet<TAttributeSet>()
            where TAttributeSet : GameplayAttributeSet
        {
            return owner.GetAttributeSet<TAttributeSet>();
        }

        internal void InitInternal(GameplayAbilitySystem abilitySystem)
        {
            this.owner = abilitySystem;
        }

        protected internal abstract void OnApply();

        protected internal virtual void OnRemove()
        {
        }
    }
}