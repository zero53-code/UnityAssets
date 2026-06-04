using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.GameplayTags;
using Zero53.Gas.Abilities;
using Zero53.Gas.AbilityTriggers;
using Zero53.Gas.AttributeSets;
using Zero53.Gas.Effects;
using Zero53.Utils.Attributes;

namespace Zero53.Gas
{
    [DisallowMultipleComponent]
    public class AbilitySystem : MonoBehaviour
    {
        #region 序列化

        [OdinSerialize, SerializeField]
        [OnCollectionChanged("BeforeAbilitiesChange", "AfterAbilitiesChange")]
        // [LabelIcon(guid: "aac75bf07cb097640819d1d102c8d3b4")]
        private List<AbilityInfo> abilities = new();

        [OdinSerialize, SerializeReference] 
        // [LabelIcon(guid: "6f14c79b09e2dba418ec247c90766138")]
        private AttributeSet[] attributeSetArray;

        [field: SerializeField]
        // [field: LabelIcon(guid: "d64403f63082071429603d00539686a7")]
        public TagContainer tags { get; private set; }
        
        [SerializeReference, PropertyOrder(order: 1)] 
        // [field: LabelIcon(guid: "e3690992982611f48b01d88a57537d37")]
        private List<IGameplayEffect> effects = new();
        
        #endregion

        #region Runtime Fields

        private readonly Dictionary<Type, AbilityInfo> _typeToAbilityInfo = new();

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

        public TAbility GiveAbility<TAbility>(AbilityTrigger trigger) where TAbility : GameplayAbility, new()
        {
            return (TAbility)GiveAbility(trigger, ScriptableObject.CreateInstance<TAbility>());
        }
        
        public TAbility GiveAbilityAndActivateOnce<TAbility>(AbilityTrigger trigger) where TAbility : GameplayAbility, new()
        {
            var ability = GiveAbility<TAbility>(trigger);

            if (ability != null)
            {
                ability.Commit();
            }
            
            return ability;
        }
        
        public GameplayAbility GiveAbility(AbilityTrigger trigger, GameplayAbility newAbility)
        {
            if (newAbility == null) return null;
            
            foreach (var info in abilities)
            {
                if (info.ability == newAbility) return null;
                if (info.ability.GetType() == newAbility.GetType()) return null;
            }

            var abilityInfo = new AbilityInfo(trigger, newAbility);
            abilities.Add(abilityInfo);
            HandleGaveAbility(abilityInfo);
            return abilityInfo.ability;
        }

        public void CancelAbility<TAbility>() where TAbility : GameplayAbility
        {
            _typeToAbilityInfo[typeof(TAbility)].ability.Cancel();
        }
        
        public bool RemoveAbility<TAbility>() where TAbility : GameplayAbility
        {
            if (!_typeToAbilityInfo.TryGetValue(typeof(TAbility), out var abilityInfo))
            {
                return false;
            }

            return abilities.RemoveAll(info =>
            {
                if (info.ability == abilityInfo.ability) return false;
                
                HandleRemovedAbility(abilityInfo);
                
                return true;
            }) != 0;
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
            Setup();
        }
        
        private readonly List<AbilityInfo> _abilitiesBuffer  = new();
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

        private void Setup()
        {
            foreach (var info in abilities)
            {
                HandleGaveAbility(info);
            }
            
            foreach (var attributeSet in attributeSetArray)
            {
                attributeSet.abilitySystem = this;
            }
        }

        private void AbilitiesUpdate()
        {
            // 尝试使用触发器激活技能
            foreach (var info in _abilitiesBuffer)
            {
                info.taskDomain.Update(Time.deltaTime);
                info.trigger?.Update(Time.deltaTime);
                info.ability.TryActivate();
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
                effect?.Apply(this, Time.deltaTime);
            }
        }

        private void HandleGaveAbility(AbilityInfo abilityInfo)
        {
            abilityInfo.Init();
            abilityInfo.ability.abilitySystem = this;
            abilityInfo.ability.OnGive();
            _typeToAbilityInfo[abilityInfo.ability.GetType()] = abilityInfo;
        }

        private void HandleRemovedAbility(AbilityInfo abilityInfo)
        {
            if (abilityInfo.ability.isActivated) abilityInfo.ability.Cancel();
            
            _typeToAbilityInfo.Remove(abilityInfo.ability.GetType());
            abilityInfo.ability.OnRemove();
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
                    abilities[info.Index].ability.Cancel();
                    break;
                
                case CollectionChangeType.Clear:
                    abilities.ForEach(abilityInfo => abilityInfo.ability.Cancel());
                    break;
            }
        }

        private void AfterAbilitiesChange(CollectionChangeInfo info)
        {
            switch (info.ChangeType)
            {
                case CollectionChangeType.RemoveKey
                    or CollectionChangeType.RemoveIndex
                    or CollectionChangeType.RemoveValue:
                    
                    break;
            }
            
        }
        
#endif

        #endregion
    }
}