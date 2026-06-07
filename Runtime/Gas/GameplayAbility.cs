using System;
using UnityEngine;
using Zero53.Gas.Triggers;

namespace Zero53.Gas.Abilities
{
    [Serializable]
    public abstract class GameplayAbility : ScriptableObject
    {
        public AbilitySystem abilitySystem { get; private set; }
        
        public AbilityTrigger trigger { get; private set; }
        
        public AbilityTaskDomain domain { get; private set; }
        
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

        internal void TryActivate()
        {
            if (trigger is { isActive: false }) return;
            
            var primaryTask = Commit();
            if (primaryTask != null)
            {
                domain.AddAbilityTask(primaryTask);
            }
            
            if (primaryTask == null)
            {
                OnEnd(null);
            }
        }

        internal void InitInternal(AbilitySystem abilitySystem, AbilityTrigger trigger, AbilityTaskDomain domain)
        {
            Debug.Log(GetType().Name + " Init");
            this.abilitySystem = abilitySystem;
            this.trigger = trigger;
            this.domain = domain;
        }
        
        /// <summary>
        /// 提交技能
        /// </summary>
        protected internal abstract AbilityTask Commit();

        /// <summary>
        /// 获取技能时调用
        /// </summary>
        protected internal virtual void OnGive()
        {
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
        protected internal virtual void OnCancel(AbilityTask rootTask)
        {
        }

        /// <summary>
        /// 技能结束、取消时调用
        /// </summary>
        protected internal virtual void OnEnd(AbilityTask rootTask)
        {
        }

        public override string ToString()
        {
            return GetType().FullName;
        }
    }
}