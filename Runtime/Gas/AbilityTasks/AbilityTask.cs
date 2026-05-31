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
        public GameplayAbility ability => domain.ability;
        public AbilitySystem abilitySystem => domain.abilitySystem;
        
        public AbilityTask parentTask { get; internal set; }

        /// <summary>
        /// 子任务
        /// </summary>
        [SerializeReference] private List<AbilityTask> subTasks = new();

        /// <summary>
        /// 任务是否结束
        /// </summary>
        [field: SerializeField]
        public bool isCanceled { get; private set; }

        /// <summary>
        /// 任务是否结束
        /// </summary>
        [field: SerializeField]
        public bool isEnded { get; private set; }

        /// <summary>
        /// 当任务被添加到 AbilitySystem 中时调用
        /// </summary>
        internal void Init()
        {
            isCanceled = false;
            isEnded = false;
            OnInit();
        }

        protected internal virtual void OnInit()
        {
        }

        private List<AbilityTask> _subTasksBuffer = new();
        internal void Update(float deltaTime)
        {
            _subTasksBuffer.Clear();
            _subTasksBuffer.AddRange(subTasks);
            
            foreach (var task in _subTasksBuffer)
            {
                task.domain = domain;
                task.parentTask = this;
                
                task.Update(deltaTime);
            }
            
            OnUpdate(deltaTime);
        }

        /// <summary>
        /// 当任务被添加到 AbilitySystem 中后每帧调用
        /// </summary>
        /// <param name="deltaTime">Time.deltaTime</param>
        protected internal abstract void OnUpdate(float deltaTime);

        /// <summary>
        /// 任务被取消后调用, 随后立刻调用 OnEnd
        /// </summary>
        protected virtual void OnCancel()
        {
        }

        /// <summary>
        /// 任务结束后调用
        /// </summary>
        protected virtual void OnEnd()
        {
        }

        /// <summary>
        /// 取消任务, 并取消所有子任务
        /// </summary>
        public void Cancel()
        {
            if (!domain.CancelAbilityTask(this)) return;

            isCanceled = true;
            foreach (var subTask in subTasks)
            {
                subTask.Cancel();
            }

            OnCancel();
            End();
        }

        /// <summary>
        /// 结束任务, 并结束所有子任务
        /// </summary>
        protected void End()
        {
            if (isEnded) return;

            isEnded = true;
            foreach (var subTask in subTasks)
            {
                subTask.End();
            }

            domain.tasks.Remove(this);

            OnEnd();
        }

        /// <summary>
        /// 添加子任务
        /// </summary>
        /// <typeparam name="TTask">任务类型</typeparam>
        protected void AddSubTask<TTask>() where TTask : AbilityTask, new()
        {
            AddSubTask(new TTask());
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
            task.parentTask = this;
            
            subTasks.Add(task);
            task.Init();
        }
    }
}