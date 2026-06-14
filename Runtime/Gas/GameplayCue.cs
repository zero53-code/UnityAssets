using System;

namespace Zero53.Gas
{
    [Serializable]
    public abstract class GameplayCue
    {
        public GameplayAbilitySystem owner { get; private set; }

        internal void InitInternal(GameplayAbilitySystem abilitySystem)
        {
            owner = abilitySystem;
        }
        
        protected internal virtual void OnStart() 
        {}
        
        protected internal virtual void OnUpdate(float deltaTime)
        {}
        
        protected internal virtual void OnRemove()
        {}
    }
}