using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Zero53.Utils;

namespace Zero53.Gas
{
    [Serializable]
    public sealed class GameplayAbilityTaskDomain
    {
        public GameplayAbility ability { get; private set; }
        public GameplayAbilitySystem abilitySystem => ability.abilitySystem;
        
        /// <summary>
        /// 以树形结构存储任务 
        /// </summary>
        [SerializeReference]
        internal List<GameplayAbilityTask> activatedAbilityTasks = new();
        
        private readonly List<GameplayAbilityTask> _activatedAbilityTasksBuffer = new();

        internal void Init(GameplayAbility ability)
        {
            this.ability = ability;
            foreach (var task in activatedAbilityTasks)
            {
                task.StartInternal(null, this);
            }
        }
        
        internal void Update(float deltaTime)
        {
            activatedAbilityTasks.RemoveAll(rootTask => !rootTask.isRunning);
            
            _activatedAbilityTasksBuffer.Clear();
            _activatedAbilityTasksBuffer.AddRange(activatedAbilityTasks);
            
            foreach (var task in _activatedAbilityTasksBuffer)
            {
                task.domain = this;
                
                task.UpdateInternal(deltaTime);
            }
        }
        
        internal bool AddAbilityTask<T>(T task) where T : GameplayAbilityTask
        {
            return AddAbilityTask((GameplayAbilityTask)task);
        }

        internal bool AddAbilityTask(GameplayAbilityTask task)
        {
            activatedAbilityTasks.Add(task);
            task.StartInternal(null, this);
            return true;
        }

        internal bool CancelAbilityTask<T>(T task) where T : GameplayAbilityTask
        {
            if (task.isEnded) return false;
            if (task.isCanceled) return false;
            if (!activatedAbilityTasks.Contains(task)) return false;
            
            task.Cancel();
            activatedAbilityTasks.Remove(task);
            return true;
        }

        internal void CancelAllAbilityTasks()
        {
            foreach (var task in activatedAbilityTasks)
            {
                task.Cancel();
            }
            
            activatedAbilityTasks.Clear();
        }

        public bool anyAbilityTaskRunning
        {
            get
            {
                foreach (var task in activatedAbilityTasks)
                {
                    if (task.isRunning) return true;
                }
                
                return false;
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            var stack = new List<GameplayAbilityTask>();
            stack.AddRange(activatedAbilityTasks);
            while (stack.Count > 0)
            {
                var task = stack[^1];
                stack.RemoveAt(stack.Count - 1);
                task.InvokeOnDrawGizmos();
                stack.AddRange(task.subTasks);
            }
        }
        
        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmosSelected()
        {
            var stack = new List<GameplayAbilityTask>();
            stack.AddRange(activatedAbilityTasks);
            while (stack.Count > 0)
            {
                var task = stack[^1];
                stack.RemoveAt(stack.Count - 1);
                task.InvokeOnDrawGizmos();
                stack.AddRange(task.subTasks);
            }
        }
    }
    
#if UNITY_EDITOR
    
    public class AbilityTaskDomainDrawer : OdinValueDrawer<GameplayAbilityTaskDomain>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (ValueEntry.SmartValue.ability == null)
            {
                CallNextDrawer(label);
                return;
            }
            
            var typeName = ValueEntry.SmartValue.ability.GetType().FullName;

            var tasksProperty = Property.Children["activatedAbilityTasks"];
            tasksProperty.Label = new GUIContent($"Task domain of {typeName}");
            tasksProperty.Draw();
        }
    }
    
#endif
}