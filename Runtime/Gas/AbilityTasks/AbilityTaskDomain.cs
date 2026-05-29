using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTasks
{
    [Serializable]
    public sealed class AbilityTaskDomain
    {
        public AbilitySystem abilitySystem { get; internal set; }
        public GameplayAbility ability { get; internal set; }
        
        [SerializeReference]
        private List<AbilityTask> tasks = new();
        
        private readonly List<AbilityTask> _tasksAddPending = new();
        private readonly List<AbilityTask> _tasksCancelPending = new();
        
        internal void OnUpdate(float deltaTime)
        {
            TasksAddPendingUpdate();
            TasksCancelPendingUpdate();
            RemoveAllEndedTask();
            
            foreach (var task in tasks)
            {
                task.OnUpdate(deltaTime);
            }
        }
        
        public bool AddAbilityTask<T>(T task) where T : AbilityTask
        {
            task.ability = ability;
            if (task.isEnded) return false;
            if (_tasksAddPending.Contains(task)) return false;
            task.rootTask = task;
            
            _tasksAddPending.Add(task);
            return true;
        }

        public bool CancelAbilityTask<T>(T task) where T : AbilityTask
        {
            if (task.isEnded) return false;
            if (task.isCanceled) return false;
            if (_tasksCancelPending.Contains(task)) return false;
            
            _tasksCancelPending.Add(task);
            return true;
        }
        
        private void TasksAddPendingUpdate()
        {
            tasks.AddRange(_tasksAddPending);
            foreach (var task in _tasksAddPending)
            {
                task.abilitySystem = abilitySystem;
            }
            _tasksAddPending.Clear();
        }

        private void TasksCancelPendingUpdate()
        {
            tasks.RemoveAll(task =>
            {
                if (!_tasksCancelPending.Contains(task)) return false;
                
                CancelTask(task);
                return true;
            });
            _tasksCancelPending.Clear();
        }

        private void RemoveAllEndedTask()
        {
            tasks.RemoveAll(task =>
            {
                if (!task.isEnded) return false;
                
                EndTask(task);
                return true;
            });
        }

        private static void CancelTask(AbilityTask task)
        {
            task.OnCancel();
            task.OnEnd();
            if (task.parentTask != null) return;
                
            task.ability?.OnCancel();
            task.ability?.OnEnd();
        }

        private static void EndTask(AbilityTask task)
        {
            task.OnEnd();
            if (task.parentTask != null) return;
                
            task.ability?.OnEnd();
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

            var tasksProperty = Property.Children["tasks"];
            tasksProperty.Label = new GUIContent($"Task domain of {typeName}");
            tasksProperty.Draw();
        }
    }
    
#endif
}