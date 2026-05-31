using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.Gas.AbilityTasks;
using Zero53.Gas.AbilityTriggers;

namespace Zero53.Gas.Abilities
{
    [Serializable]
    public abstract class GameplayAbility
    {
        [field: OdinSerialize, SerializeReference, BoxGroup]
        public AbilityTrigger trigger { get; set; }
        
        [field: SerializeField]
        public AbilityTaskDomain domain { get; internal set; }
        
        public AbilitySystem abilitySystem { get; internal set; }
        

        /// <summary>
        /// 技能是否正在执行
        /// </summary>
        public bool isExecuting => throw new NotImplementedException();

        public bool isEnded => !isExecuting;

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行技能
        /// </summary>
        protected internal abstract void Execute();

        internal void OnGiveBefore()
        {
            trigger.ability = this;
        }
        
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
        /// 执行技能前调用
        /// </summary>
        protected internal virtual void OnPreExecute()
        {
        }

        /// <summary>
        /// 技能被取消执行时调用
        /// </summary>
        protected internal virtual void OnCancel()
        {
        }

        /// <summary>
        /// 技能结束、取消、打断时调用
        /// </summary>
        protected internal virtual void OnEnd()
        {
        }

#if UNITY_EDITOR

        [Button]
        private void ExecuteAbility()
        {
            if (!Application.isPlaying) return;

            abilitySystem.ExecuteAbility(this);
        }

#endif
    }
}