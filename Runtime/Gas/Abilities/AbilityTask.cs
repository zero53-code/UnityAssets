using System;
using UnityEngine;

namespace Zero53.Gas.Abilities
{
    [Serializable]
    public abstract class AbilityTask
    {
        public AbilitySystem abilitySystem { get; internal set; }
        public IGameplayAbility ability { get; internal set; }
        
        [field: SerializeField]
        public bool isEnd { get; private set; }

        protected AbilityTask(IGameplayAbility ability)
        {
            this.ability = ability;
        }
        
        protected virtual void Init() {}
        
        protected internal abstract void OnUpdate(float deltaTime);
        
        protected internal virtual void OnCancel() {}
        
        protected internal virtual void OnEnd() {}

        public void Cancel()
        {
            if (isEnd) return;
            
            CancelTask(this);
        }

        protected void End()
        {
            if (!isEnd) return;
            
            isEnd = true;
            OnEnd();
        }

        protected void AddTask<T>(T task) where T : AbilityTask
        {
            task.abilitySystem = abilitySystem;
            task.ability = ability;
            
            if (abilitySystem.AddAbilityTask(task))
            {
                Init();
            }
        }

        public void CancelTask<T>(T task) where T : AbilityTask
        {
            abilitySystem.CancelAbilityTask(task);
        }
    }
}