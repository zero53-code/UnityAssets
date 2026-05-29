using System;
using System.Collections.Generic;
using UnityEngine;
using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTasks
{
    /// <summary>
    /// 任务基类
    /// </summary>
    [Serializable]
    public abstract class AbilityTask
    {
        public AbilityTaskDomain domain { get; internal set; }
        public AbilitySystem abilitySystem { get; internal set; }
        public GameplayAbility ability { get; internal set; }
     
        /// <summary>
        /// 父任务
        /// </summary>
        public AbilityTask parentTask { get; private set; }
        
        public AbilityTask rootTask { get; internal set; }
        
        /// <summary>
        /// 子任务
        /// </summary>
        [SerializeReference] private List<AbilityTask> subTasks = new();
        
        /// <summary>
        /// 任务是否结束
        /// </summary>
        [field: SerializeField] public bool isCanceled { get; private set; }
        
        /// <summary>
        /// 任务是否结束
        /// </summary>
        [field: SerializeField] public bool isEnded { get; private set; }
        
        /// <summary>
        /// 当任务被添加到 AbilitySystem 中时调用
        /// </summary>
        protected virtual void Init() {}
        
        /// <summary>
        /// 当任务被添加到 AbilitySystem 中后每帧调用
        /// </summary>
        /// <param name="deltaTime">Time.deltaTime</param>
        protected internal abstract void OnUpdate(float deltaTime);
        
        /// <summary>
        /// 任务被取消后的下一帧调用, 随后立刻调用 OnEnd
        /// </summary>
        protected internal virtual void OnCancel() {}
        
        /// <summary>
        /// 任务结束后的下一帧调用
        /// </summary>
        protected internal virtual void OnEnd() {}

        /// <summary>
        /// 取消任务, 并取消所有子任务
        /// 可由外部调用
        /// </summary>
        public void Cancel()
        {
            if (!domain.CancelAbilityTask(this)) return;
            
            isCanceled = true;
            foreach (var subTask in subTasks)
            {
                subTask.Cancel();
            }
            
            End();
        }

        /// <summary>
        /// 结束任务, 并结束所有子任务
        /// 仅在内部调用
        /// </summary>
        protected void End()
        {
            if (isEnded) return;
            
            isEnded = true;
            foreach (var subTask in subTasks)
            {
                subTask.End();
            }
        }

        /// <summary>
        /// 添加子任务
        /// </summary>
        /// <param name="task">任务对象</param>
        /// <typeparam name="TTask">任务类型</typeparam>
        protected void AddSubTask<TTask>(TTask task) where TTask : AbilityTask
        {
            AddSubTask((AbilityTask)task);
        }

        /// <summary>
        /// 添加子任务
        /// </summary>
        /// <param name="task">任务对象</param>
        protected void AddSubTask(AbilityTask task)
        {
            task.domain = domain;
            task.abilitySystem = abilitySystem;
            task.ability = ability;
            task.rootTask = rootTask;
            task.parentTask = this;
            subTasks.Add(task);
            
            if (domain.AddAbilityTask(task))
            {
                Init();
            }
        }
    }
}