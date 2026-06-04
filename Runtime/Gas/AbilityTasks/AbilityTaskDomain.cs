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
        public GameplayAbility ability { get; internal set; }
        public AbilitySystem abilitySystem => ability.abilitySystem;
        
        /// <summary>
        /// 以树形结构存储任务 
        /// </summary>
        [SerializeReference]
        internal List<AbilityTask> rootTasks = new();
        
        private List<AbilityTask> _rootTasksBuffer = new();

        internal void Init(GameplayAbility ability)
        {
            this.ability = ability;
            foreach (var task in rootTasks)
            {
                task.Start(null, this);
            }
        }
        
        internal void Update(float deltaTime)
        {
            rootTasks.RemoveAll(rootTask => !rootTask.isRunning);
            
            _rootTasksBuffer.Clear();
            _rootTasksBuffer.AddRange(rootTasks);
            
            foreach (var task in _rootTasksBuffer)
            {
                task.domain = this;
                
                task.Update(deltaTime);
            }
        }
        
        internal bool AddAbilityTask<T>(T task) where T : AbilityTask
        {
            return AddAbilityTask((AbilityTask)task);
        }

        internal bool AddAbilityTask(AbilityTask task)
        {
            rootTasks.Add(task);
            task.Start(null, this);
            return true;
        }

        internal bool CancelAbilityTask<T>(T task) where T : AbilityTask
        {
            if (task.isEnded) return false;
            if (task.isCanceled) return false;
            if (!rootTasks.Contains(task)) return false;
            
            task.Cancel();
            rootTasks.Remove(task);
            return true;
        }

        internal void CancelAllAbilityTasks()
        {
            foreach (var task in rootTasks)
            {
                task.Cancel();
            }
            
            rootTasks.Clear();
        }

        public bool anyAbilityTaskRunning
        {
            get
            {
                foreach (var task in rootTasks)
                {
                    if (task.isRunning) return true;
                }
                
                return false;
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

            var tasksProperty = Property.Children["rootTasks"];
            tasksProperty.Label = new GUIContent($"Task domain of {typeName}");
            tasksProperty.Draw();
        }
    }
    
#endif
}