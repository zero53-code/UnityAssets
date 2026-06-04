using System;
using UnityEngine;
using Zero53.Gas.AbilityTasks;
using Zero53.Gas.AbilityTriggers;

namespace Zero53.Gas.Abilities
{
    [Serializable]
    public abstract class GameplayAbility : ScriptableObject
    {
        public AbilityTriggerBase trigger { get; set; }
        
        public AbilityTaskDomain domain { get; internal set; }
        
        public AbilitySystem abilitySystem { get; internal set; }
        
        /// <summary>
        /// 技能是否正在执行
        /// </summary>
        public bool isActivated => domain.anyAbilityTaskRunning;

        public bool isEnded => !isActivated;

        public void Cancel()
        {
            domain.CancelAllAbilityTasks();
        }

        internal void TryActivate()
        {
            if (trigger is { isActive: false }) return;
            
            OnPreCommit();
            
            var primaryTask = Commit();
            if (primaryTask != null)
            {
                domain.AddAbilityTask(primaryTask);
            }
            
            OnPostCommit();
            
            if (primaryTask == null)
            {
                OnEnd(null);
            }
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
        /// 提交技能前调用
        /// </summary>
        protected internal virtual void OnPreCommit()
        {
        }
        
        protected internal virtual void OnPostCommit()
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
    }
}