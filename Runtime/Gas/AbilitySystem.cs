using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.GameplayTags;
using Zero53.Gas.Abilities;
using Zero53.Gas.AbilityTasks;
using Zero53.Gas.AttributeSets;
using Zero53.Gas.Effects;
using Zero53.Utils.Attributes;

namespace Zero53.Gas
{
    [DisallowMultipleComponent]
    public class AbilitySystem : MonoBehaviour
    {
        #region 序列化

        [OdinSerialize, SerializeReference]
        [OnCollectionChanged("BeforeAbilitiesChange", "AfterAbilitiesChange")]
        [LabelIcon(guid: "aac75bf07cb097640819d1d102c8d3b4")]
        private List<GameplayAbility> abilities = new();

        [OdinSerialize, SerializeReference] 
        [LabelIcon(guid: "6f14c79b09e2dba418ec247c90766138")]
        private AttributeSet[] attributeSetArray;

        [field: SerializeField]
        [field: LabelIcon(guid: "d64403f63082071429603d00539686a7")]
        public TagContainer tags { get; private set; }
        
        [SerializeReference, PropertyOrder(order: 1)] 
        private List<IGameplayEffect> effects = new();
        
        #endregion

        #region API

        public TAttributeSet GetAttributeSet<TAttributeSet>() where TAttributeSet : AttributeSet
        {
            foreach (var attributeSet in attributeSetArray)
            {
                if (attributeSet.GetType() == typeof(TAttributeSet)) 
                    return (TAttributeSet) attributeSet;
            }
            
            return null;
        }

        public bool GiveAbility<TAbility>() where TAbility : GameplayAbility, new()
        {
            return GiveAbility(new TAbility());
        }
        
        public bool GiveAbility(GameplayAbility newAbility)
        {
            if (newAbility == null || abilities.Contains(newAbility)) return false;
            
            foreach (var ability in abilities)
            {
                if (ability.GetType() == newAbility.GetType()) return false;
            }

            newAbility.abilitySystem = this;
            CreateTaskDomain(newAbility);
            abilities.Add(newAbility);
            return true;
        }

        public void CancelAbility<TAbility>() where TAbility : GameplayAbility
        {
            foreach (var ability in abilities)
            {
                if (ability.GetType() == typeof(TAbility)) continue;
                
                ability.Cancel();
                return;
            }
        }
        
        public bool RemoveAbility<TAbility>() where TAbility : GameplayAbility
        {
            var ability = abilities.FirstOrDefault(ability => ability is TAbility);
            return ability != null && abilities.Remove(ability);
        }

        internal void ExecuteAbility<TAbility>() where TAbility : GameplayAbility
        {
            foreach (var ability in abilities)
            {
                if (ability.GetType() == typeof(TAbility)) continue;
                
                ExecuteAbility(ability);
                return;
            }
        }

        internal void ExecuteAbility(GameplayAbility ability)
        {
            ability.Execute();
            ability.OnPreExecute();
        }

        #endregion

        #region Effects API

        public void AddEffect(IGameplayEffect effect)
        {
            effects.Add(effect);
        }

        public void AddEffects(IEnumerable<IGameplayEffect> effects)
        {
            this.effects.AddRange(effects);
        }

        public bool RemoveEffect(IGameplayEffect effect)
        {
            return effects.Remove(effect);
        }

        public void ClearEffects()
        {
            effects.Clear();
        }

        #endregion

        #region Unity 生命周期

        private void Awake()
        {
            foreach (var attributeSet in attributeSetArray)
            {
                attributeSet.abilitySystem = this;
            }

            foreach (var ability in abilities)
            {
                ability.abilitySystem = this;
                CreateTaskDomain(ability);
            }
        }
        private readonly List<GameplayAbility> _abilitiesBuffer  = new();
        private readonly List<AttributeSet> _attributeSetsBuffer = new();
        private readonly List<IGameplayEffect> _effectsBuffer = new();

        private void Update()
        {
            _abilitiesBuffer.Clear();
            _abilitiesBuffer.AddRange(abilities);
            _attributeSetsBuffer.Clear();
            _attributeSetsBuffer.AddRange(attributeSetArray);
            _effectsBuffer.Clear();
            _effectsBuffer.AddRange(effects);
            AbilitiesUpdate();
            AttributeSetsUpdate();
            EffectsUpdate();
        }

        #endregion

        #region 私有方法

        private void AbilitiesUpdate()
        {
            foreach (var ability in _abilitiesBuffer)
            {
                ability.domain.OnUpdate(Time.deltaTime);
                if (ability.trigger?.Check(Time.deltaTime) ?? false)
                {
                    ability.Execute();
                }
            }
        }

        private void AttributeSetsUpdate()
        {
            foreach (var attributeSet in _attributeSetsBuffer)
            {
                attributeSet.Update();
            }
        }

        private void EffectsUpdate()
        {
            foreach (var effect in _effectsBuffer)
            {
                effect.Apply(this, Time.deltaTime);
            }
        }
        
        private static void CreateTaskDomain(GameplayAbility ability)
        {
            var taskDomain = new AbilityTaskDomain();
            
            ability.domain = taskDomain;
            taskDomain.ability = ability;
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