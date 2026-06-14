using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Zero53.Gas
{
    /// <summary>
    /// 任务基类
    /// </summary>
    [Serializable]
    public abstract class GameplayAbilityTask
    {
        public GameplayAbilityTaskDomain domain { get; internal set; }
        public GameplayAbilityBase ability => domain.ability;
        public GameplayAbilitySystem abilitySystem => domain.abilitySystem;
        
        [CanBeNull] public GameplayAbilityTask parentTask { get; internal set; }

        /// <summary>
        /// 子任务
        /// </summary>
        [SerializeReference] internal List<GameplayAbilityTask> subTasks = new();

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
        /// 任务是否正在运行
        /// </summary>
        public bool isRunning => !isCanceled && !isEnded;

        /// <summary>
        /// 当任务开始时调用
        /// </summary>
        internal void StartInternal([CanBeNull] GameplayAbilityTask parentTask, GameplayAbilityTaskDomain domain)
        {
            this.parentTask = parentTask;
            this.domain = domain;
            OnStart();
            
            foreach (var subTask in subTasks)
            {
                subTask.StartInternal(this, domain);
            }
        }
        
        /// <summary>
        /// 当任务开始时调用
        /// </summary>
        protected virtual void OnStart()
        {
        }

        private List<GameplayAbilityTask> _subTasksBuffer = new();
        internal void UpdateInternal(float deltaTime)
        {
            subTasks.RemoveAll(subTask => !subTask.isRunning);
            
            _subTasksBuffer.Clear();
            _subTasksBuffer.AddRange(subTasks);
            
            foreach (var task in _subTasksBuffer)
            {
                task.UpdateInternal(deltaTime);
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
            isCanceled = true;
            foreach (var subTask in subTasks)
            {
                subTask.Cancel();
            }

            OnCancel();
            End();

            if (parentTask == null)
            {
                ability.OnCancel(this);
            }
        }

        /// <summary>
        /// 结束任务, 并结束所有子任务
        /// </summary>
        protected void End()
        {
            if (isEnded) return;

            isEnded = true;
            
            _subTasksBuffer.Clear();
            _subTasksBuffer.AddRange(subTasks);
            foreach (var subTask in _subTasksBuffer)
            {
                subTask.End();
            }

            OnEnd();
        }

        /// <summary>
        /// 添加子任务
        /// </summary>
        /// <typeparam name="TTask">任务类型</typeparam>
        protected void AddSubTask<TTask>() where TTask : GameplayAbilityTask, new()
        {
            AddSubTask(new TTask());
        }

        /// <summary>
        /// 添加子任务
        /// </summary>
        /// <param name="task">任务对象</param>
        /// <typeparam name="TTask">任务类型</typeparam>
        protected void AddSubTask<TTask>(TTask task) where TTask : GameplayAbilityTask
        {
            AddSubTask((GameplayAbilityTask)task);
        }

        /// <summary>
        /// 添加子任务
        /// </summary>
        /// <param name="task">任务对象</param>
        protected void AddSubTask(GameplayAbilityTask task)
        {
            subTasks.Add(task);
            task.StartInternal(this, domain);
        }

        private void SetChildTask(GameplayAbilityTask task)
        {
            task.parentTask = this;
            foreach (var subTask in task.subTasks)
            {
                subTask.SetChildTask(task);
            }
        }
    }
}