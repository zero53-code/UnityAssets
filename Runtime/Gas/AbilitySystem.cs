using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.GameplayTags;
using Zero53.Gas.Abilities;
using Zero53.Gas.AbilityTasks;
using Zero53.Gas.AttributeSet;

namespace Zero53.Gas
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GameplayAttributeSet))]
    [RequireComponent(typeof(Tags))]
    public class AbilitySystem : MonoBehaviour
    {
        #region 序列化

        [OdinSerialize, SerializeReference]
        [OnCollectionChanged("BeforeAbilitiesChange", "AfterAbilitiesChange")]
        private List<GameplayAbility> abilities = new();
        
        [SerializeField, ReadOnly]
        private List<AbilityTaskDomain> taskDomains = new();

        #endregion

        #region API

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

        public bool AddAbility<TAbility>() where TAbility : GameplayAbility, new()
        {
            _abilitiesRemovePending.RemoveAll(ability => ability is TAbility);
            
            if (abilities.Any(ability => ability is TAbility)) return true;
            
            var ability = new TAbility
            {
                abilitySystem = this
            };

            CreateTaskDomain(ability);

            _abilitiesAddPending.Add(ability);
            return true;
        }

        public bool RemoveAbility<TAbility>() where TAbility : GameplayAbility
        {
            _abilitiesAddPending.RemoveAll(ability => ability is TAbility);
            
            var ability = abilities.FirstOrDefault(ability => ability is TAbility);
            if (ability == null) return false;
            
            _abilitiesRemovePending.Add(ability);
            return true;

        }

        public void ExecuteAbility<TAbility>() where TAbility : GameplayAbility
        {
            foreach (var ability in abilities)
            {
                if (ability is not TAbility) continue;
                
                ExecuteAbility(ability);
                return;
            }
        }

        public void ExecuteAbility(GameplayAbility ability)
        {
            _abilitiesExecutePending.Add(ability);
            ability.OnPreExecute();
        }

        public void CancelAbility<TAbility>() where TAbility : GameplayAbility
        {
            foreach (var ability in _abilitiesAddPending)
            {
                if (ability is not TAbility) continue;
                
                ability.Cancel();
                return;
            }

            foreach (var ability in abilities)
            {
                if (ability is not TAbility) continue;
                
                ability.Cancel();
                return;
            }
        }

        #endregion
        
        #region 私有字段

        private readonly List<GameplayAbility> _abilitiesAddPending = new();
        private readonly List<GameplayAbility> _abilitiesRemovePending = new();
        private readonly List<GameplayAbility> _abilitiesExecutePending = new();
        private GameplayAttributeSet _attributeSet;
        private Tags _tags;

        #endregion

        #region Unity 生命周期

        private void Awake()
        {
            foreach (var ability in abilities)
            {
                ability.abilitySystem = this;
                CreateTaskDomain(ability);
            }
        }

        private void Start()
        {
            _attributeSet = GetComponent<GameplayAttributeSet>();
            _tags = GetComponent<Tags>();
        }

        private void Update()
        {
            AbilitiesUpdate();

            foreach (var taskDomain in taskDomains)
            {
                taskDomain.OnUpdate(Time.deltaTime);
            }
        }

        #endregion

        #region 私有方法

        private void AbilitiesUpdate()
        {
            foreach (var ability in abilities)
            {
                if (ability.trigger?.Check(Time.deltaTime) ?? false)
                {
                    ability.Execute();
                }
            }
            
            AbilitiesAddPendingUpdate();
            AbilitiesRemovePendingUpdate();
            AbilitiesExecutePendingUpdate();
        }
        
        private void AbilitiesAddPendingUpdate()
        {
            abilities.AddRange(_abilitiesAddPending);
            foreach (var ability in _abilitiesAddPending)
            {
                ability.OnGiveBefore();
                ability.OnGive();
            }
            _abilitiesAddPending.Clear();
        }
        
        private void AbilitiesRemovePendingUpdate()
        {
            abilities.RemoveAll(ability =>
            {
                if (!_abilitiesRemovePending.Contains(ability)) return false;
                
                taskDomains.RemoveAll(taskDomain => ReferenceEquals(taskDomain.ability, ability));
                
                return true;
            });
            
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

        private void CreateTaskDomain(GameplayAbility ability)
        {
            var taskDomain = new AbilityTaskDomain
            {
                abilitySystem = this,
            };
            
            ability.domain = taskDomain;
            taskDomain.ability = ability;
            
            taskDomains.Add(taskDomain);
        }
        
        #endregion

        #region Editor

#if UNITY_EDITOR

        private void BeforeAbilitiesChange(CollectionChangeInfo info)
        {
            if (!Application.isPlaying) return;

            switch (info.ChangeType)
            {
                case CollectionChangeType.RemoveKey
                    or CollectionChangeType.RemoveIndex
                    or CollectionChangeType.RemoveValue:
                    abilities[info.Index].Cancel();
                    break;
                
                case CollectionChangeType.Clear:
                    abilities.ForEach(ability => ability.Cancel());
                    break;
            }
        }

        private void AfterAbilitiesChange(CollectionChangeInfo info)
        {
            if (!Application.isPlaying) return;

            if (info.ChangeType is not (CollectionChangeType.Add
                or CollectionChangeType.Insert
                or CollectionChangeType.SetKey)) return;
            
            if (info.Value is not GameplayAbility ability) return;

            ability.abilitySystem = this;
        }
        
#endif

        #endregion
    }
}