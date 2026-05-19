using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zero53.GameplayTags;
using Zero53.Gas.Abilities;
using Zero53.Gas.Attributes;

namespace Zero53.Gas
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameplayAttributeSet))]
    [RequireComponent(typeof(Tags))]
    public class AbilitySystem : MonoBehaviour
    {
        [SerializeReference]
        private List<IGameplayAbility> abilities = new();
        
        [SerializeReference]
        private List<AbilityTask> tasks = new();
        
        public GameplayAttributeSet attributeSet
        {
            get
            {
                if (_attributeSet == null) _attributeSet = GetComponent<GameplayAttributeSet>();
                return _attributeSet;
            }
        }

        public Tags tags
        {
            get
            {
                if (_tags == null) _tags = GetComponent<Tags>();
                return _tags;
            }
        }

        public bool AddAbility<TAbility>() where TAbility : IGameplayAbility, new()
        {
            _abilitiesRemovePending.RemoveAll(ability => ability is TAbility);
            
            if (abilities.Any(ability => ability is TAbility)) return true;
            
            _abilitiesAddPending.Add(new TAbility());
            return true;
        }

        public bool RemoveAbility<TAbility>() where TAbility : IGameplayAbility
        {
            _abilitiesAddPending.RemoveAll(ability => ability is TAbility);
            
            var ability = abilities.FirstOrDefault(ability => ability is TAbility);
            if (ability == null) return false;
            
            _abilitiesRemovePending.Add(ability);
            ability.OnPreExecute();
            return true;

        }

        public void Execute<TAbility>() where TAbility : IGameplayAbility
        {
            var ability = abilities.FirstOrDefault(ability => ability is TAbility);
            if (ability == null) return;
            
            _abilitiesExecutePending.Add(ability);
        }
        
        private readonly List<AbilityTask> _tasksAddPending = new();
        private readonly List<AbilityTask> _tasksCancelPending = new();
        
        internal bool AddAbilityTask<T>(T task) where T : AbilityTask
        {
            if (task.isEnd) return false;
            if (_tasksAddPending.Contains(task)) return false;
            
            _tasksAddPending.Add(task);
            return true;
        }

        internal bool CancelAbilityTask<T>(T task) where T : AbilityTask
        {
            if (task.isEnd) return false;
            if (_tasksCancelPending.Contains(task)) return false;
            
            _tasksCancelPending.Add(task);
            return true;
        }
        
        private readonly List<IGameplayAbility> _abilitiesAddPending = new();
        private readonly List<IGameplayAbility> _abilitiesRemovePending = new();
        private readonly List<IGameplayAbility> _abilitiesExecutePending = new();
        private GameplayAttributeSet _attributeSet;
        private Tags _tags;
        
        private void Start()
        {
            _attributeSet = GetComponent<GameplayAttributeSet>();
            _tags = GetComponent<Tags>();
        }

        private void Update()
        {
            AbilitiesUpdate();
            TasksUpdate();
        }

        private void AbilitiesUpdate()
        {
            AbilitiesAddPendingUpdate();
            AbilitiesRemovePendingUpdate();
            AbilitiesExecutePendingUpdate();
        }
        
        private void AbilitiesAddPendingUpdate()
        {
            abilities.AddRange(_abilitiesAddPending);
            foreach (var ability in _abilitiesAddPending)
            {
                ability.OnGive(this);
            }
            _abilitiesAddPending.Clear();
        }
        
        private void AbilitiesRemovePendingUpdate()
        {
            abilities.RemoveAll(ability => _abilitiesRemovePending.Contains(ability));
            foreach (var ability in _abilitiesRemovePending)
            {
                ability.OnRemove();
            }
            _abilitiesRemovePending.Clear();
        }
        
        private void AbilitiesExecutePendingUpdate()
        {
            foreach (var ability in _abilitiesExecutePending)
            {
                if (!abilities.Contains(ability)) continue;
                
                ability.Execute();
            }
            _abilitiesExecutePending.Clear();
        }

        private void TasksUpdate()
        {
            TasksAddPendingUpdate();
            TasksCancelPendingUpdate();
            RemoveAllEndedTask();
            
            foreach (var task in tasks)
            {
                task.OnUpdate(Time.deltaTime);
            }
        }

        private void TasksAddPendingUpdate()
        {
            tasks.AddRange(_tasksAddPending);
            foreach (var task in _tasksAddPending)
            {
                task.abilitySystem = this;
            }
            tasks.Clear();
        }

        private void TasksCancelPendingUpdate()
        {
            tasks.RemoveAll(task =>
            {
                if (!_tasksCancelPending.Contains(task)) return false;
                
                task.OnCancel();
                task.OnEnd();
                return true;
            });
            _tasksCancelPending.Clear();
        }

        private void RemoveAllEndedTask()
        {
            tasks.RemoveAll(task =>
            {
                if (!task.isEnd) return false;
                
                task.OnEnd();
                return true;
            });
        }
    }
}