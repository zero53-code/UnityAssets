using System;

namespace Zero53.Gas
{
    [Serializable]
    public abstract class GameplayCue
    {
        public GameplayAbilitySystem abilitySystem { get; private set; }

        internal void InitInternal(GameplayAbilitySystem abilitySystem)
        {
            this.abilitySystem = abilitySystem;
        }
        
        protected internal virtual void OnStart() 
        {}
        
        protected internal abstract void OnUpdate(float deltaTime);
        
        protected internal virtual void OnRemove()
        {}
    }
}