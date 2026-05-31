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
        
        [SerializeReference]
        internal List<AbilityTask> tasks = new();
        
        private List<AbilityTask> _tasksBuffer = new();
        
        internal void OnUpdate(float deltaTime)
        {
            _tasksBuffer.Clear();
            _tasksBuffer.AddRange(tasks);
            foreach (var task in _tasksBuffer)
            {
                task.domain = this;
                
                task.Update(deltaTime);
            }
        }
        
        public bool AddAbilityTask<T>(T task) where T : AbilityTask
        {
            return AddAbilityTask((AbilityTask)task);
        }

        public bool AddAbilityTask(AbilityTask task)
        {
            task.domain = this;
            
            tasks.Add(task);
            task.Init();
            return true;
        }

        public bool CancelAbilityTask<T>(T task) where T : AbilityTask
        {
            if (task.isEnded) return false;
            if (task.isCanceled) return false;
            if (!tasks.Contains(task)) return false;
            
            task.Cancel();
            tasks.Remove(task);
            return true;
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