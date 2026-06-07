using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTasks
{
    [Serializable]
    public sealed class AbilityTaskDomain
    {
        public GameplayAbility ability { get; private set; }
        public AbilitySystem abilitySystem => ability.abilitySystem;
        
        /// <summary>
        /// 以树形结构存储任务 
        /// </summary>
        [SerializeReference]
        internal List<AbilityTask> activatedAbilityTasks = new();
        
        private readonly List<AbilityTask> _activatedAbilityTasksBuffer = new();

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
        
        internal bool AddAbilityTask<T>(T task) where T : AbilityTask
        {
            return AddAbilityTask((AbilityTask)task);
        }

        internal bool AddAbilityTask(AbilityTask task)
        {
            activatedAbilityTasks.Add(task);
            task.StartInternal(null, this);
            return true;
        }

        internal bool CancelAbilityTask<T>(T task) where T : AbilityTask
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
            var stack = new List<AbilityTask>();
            stack.AddRange(activatedAbilityTasks);
            while (stack.Count > 0)
            {
                var task = stack[^1];
                stack.RemoveAt(stack.Count - 1);
                AbilitySystem.GetOnDrawGizmosMethodInfo(task.GetType())?.Invoke(task, Array.Empty<object>());
                stack.AddRange(task.subTasks);
            }
        }
        
        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmosSelected()
        {
            var stack = new List<AbilityTask>();
            stack.AddRange(activatedAbilityTasks);
            while (stack.Count > 0)
            {
                var task = stack[^1];
                stack.RemoveAt(stack.Count - 1);
                AbilitySystem.GetOnDrawGizmosSelectedMethodInfo(task.GetType())?.Invoke(task, Array.Empty<object>());
                stack.AddRange(task.subTasks);
            }
        }
    }
    
#if UNITY_EDITOR
    
    public class AbilityTaskDomainDrawer : OdinValueDrawer<AbilityTaskDomain>
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