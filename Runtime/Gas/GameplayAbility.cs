using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.GameplayAbilityTriggers;

namespace Zero53.Gas
{
    [Serializable]
    public abstract class GameplayAbility : ScriptableObject
    {
        [field: SerializeField]
        public AbilityTrigger trigger { get; private set; }
        
        [field: SerializeField, ReadOnly]
        public GameplayAbilityTaskDomain domain { get; private set; }
        
        public GameplayAbilitySystem abilitySystem { get; private set; }
        
        /// <summary>
        /// 技能是否已激活
        /// </summary>
        public bool isActivated => domain.anyAbilityTaskRunning;

        /// <summary>
        /// 已激活的技能任务实例数量
        /// </summary>
        public int activatedCount => domain.activatedAbilityTasks.Count;

        /// <summary>
        /// 取消所有已激活的技能任务
        /// </summary>
        public void Cancel()
        {
            domain.CancelAllAbilityTasks();
        }

        /// <summary>
        /// 提交技能
        /// </summary>
        protected internal abstract GameplayAbilityTask OnCommit();

        /// <summary>
        /// 获取技能时调用
        /// </summary>
        protected internal virtual void OnGive()
        {
        }

        internal void RemoveInternal()
        {
            trigger.OnRemove();
        }

        /// <summary>
        /// 移除技能时调用
        /// </summary>
        protected internal virtual void OnRemove()
        {
        }

        /// <summary>
        /// 技能被取消执行时调用
        /// </summary>
        protected internal virtual void OnCancel([CanBeNull] GameplayAbilityTask rootTask)
        {
        }

        /// <summary>
        /// 技能结束、取消时调用
        /// </summary>
        protected internal virtual void OnEnd([CanBeNull] GameplayAbilityTask rootTask)
        {
        }

        public override string ToString()
        {
            return GetType().FullName;
        }

        private void OnDestroy()
        {
            abilitySystem.RemoveAbility(this);
        }
        
        internal void InitInternal(GameplayAbilitySystem abilitySystem)
        {
            domain ??= new GameplayAbilityTaskDomain();
            trigger ??= new AbilityTrigger();
            
            this.abilitySystem = abilitySystem;
            domain.InitInternal(this);
            trigger.InitInternal(this);
        }
        
        internal void UpdateInternal(float deltaTime)
        {
            domain.UpdateInternal(deltaTime);
            trigger.UpdateInternal(deltaTime);
            
            if (!trigger.isActive) return;
            
            var rootTask = OnCommit();
            if (rootTask != null)
            {
                domain.AddAbilityTask(rootTask);
            }
            else
            {
                OnEnd(null);
            }
        }
    }
}