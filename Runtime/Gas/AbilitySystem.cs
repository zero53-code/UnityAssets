using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        [LabelIcon(guid: "aac75bf07cb097640819d1d102c8d3b4")]
        private List<AbilityInstance> abilities = new();

        [OdinSerialize, SerializeReference] 
        [LabelIcon(guid: "6f14c79b09e2dba418ec247c90766138")]
        private AttributeSet[] attributeSets;

        [field: SerializeField]
        [field: LabelIcon(guid: "d64403f63082071429603d00539686a7")]
        public TagContainer tags { get; private set; }
        
        [SerializeReference, PropertyOrder(order: 1)] 
        [field: LabelIcon(guid: "e3690992982611f48b01d88a57537d37")]
        private List<GameplayEffect> effects = new();
        
        #endregion

        #region API

        public TAttributeSet GetAttributeSet<TAttributeSet>() where TAttributeSet : AttributeSet
        {
            foreach (var attributeSet in attributeSets)
            {
                if (attributeSet is TAttributeSet set) return set;
            }
            return null;
        }

        public TAttributeSet[] GetAttributeSets<TAttributeSet>() where TAttributeSet : AttributeSet
        {
            return attributeSets
                .OfType<TAttributeSet>()
                .ToArray();
        }

        public TAbility GiveAbility<TAbility>(AbilityTrigger trigger) where TAbility : GameplayAbility, new()
        {
            return GiveAbility(trigger, ScriptableObject.CreateInstance<TAbility>());
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
        
        public TAbility GiveAbility<TAbility>(AbilityTrigger trigger, TAbility newAbility) where TAbility : GameplayAbility
        {
            if (newAbility == null) return null;
            
            foreach (var instance in abilities)
            {
                if (instance.ability == newAbility) return null;
            }

            var abilityInstance = new AbilityInstance(trigger, newAbility);
            abilities.Add(abilityInstance);
            
            HandleGaveAbility(abilityInstance);
            
            return newAbility;
        }

        public void CancelAbility<TAbility>() where TAbility : GameplayAbility
        {
            foreach (var abilityInstance in abilities)
            {
                if (abilityInstance.ability is TAbility ability)
                {
                    ability.Cancel();
                }
            }
        }
        
        public int RemoveAbility<TAbility>() where TAbility : GameplayAbility
        {
            return abilities.RemoveAll(abilityInstance =>
            {
                if (abilityInstance.ability is not TAbility) return false;
                
                HandleRemovedAbility(abilityInstance);
                
                return true;
            });
        }

        public bool RemoveAbility<TAbility>(TAbility ability) where TAbility : GameplayAbility
        {
            var count = abilities.RemoveAll(abilityInstance =>
            {
                if (abilityInstance.ability != ability) return false;
                
                HandleRemovedAbility(abilityInstance);
                return true;

            });
            
            return count > 0;
        }
        
        #endregion
        
        #region Effects API

        public void AddEffect(GameplayEffect effect)
        {
            effects.Add(effect);
            HandleAddedEffect(effect);
        }

        public void AddEffects(IEnumerable<GameplayEffect> effects)
        {
            var gameplayEffects = effects as GameplayEffect[] ?? effects.ToArray();
            this.effects.AddRange(gameplayEffects);
            foreach (var effect in gameplayEffects)
            {
                HandleAddedEffect(effect);
            }
        }

        public bool RemoveEffect(GameplayEffect effect)
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
        
        private readonly List<AbilityInstance> _abilitiesBuffer  = new();
        private readonly List<AttributeSet> _attributeSetsBuffer = new();
        private readonly List<GameplayEffect> _effectsBuffer = new();

        private void Update()
        {
            _abilitiesBuffer.Clear();
            _abilitiesBuffer.AddRange(abilities);
            
            _attributeSetsBuffer.Clear();
            _attributeSetsBuffer.AddRange(attributeSets);
            
            _effectsBuffer.Clear();
            _effectsBuffer.AddRange(effects);
            
            AttributeSetsUpdate();
            EffectsUpdate();
            AbilitiesUpdate();
        }

        private void OnDestroy()
        {
            foreach (var abilityInstance in abilities)
            {
                HandleRemovedAbility(abilityInstance);
                Destroy(abilityInstance.ability);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Setup();
        }

        private static readonly Dictionary<Type, MethodInfo> _typeToOnDrawGizmosMethod = new();
        private static readonly Dictionary<Type, MethodInfo> _typeToOnDrawGizmosSelectedMethod = new();
        
        private void OnDrawGizmos()
        {
            foreach (var info in abilities)
            {
                GetOnDrawGizmosMethodInfo(info.ability.GetType())?.Invoke(info.ability, Array.Empty<object>());
                GetOnDrawGizmosMethodInfo(info.taskDomain.GetType())?.Invoke(info.taskDomain, Array.Empty<object>());
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var info in abilities)
            {
                GetOnDrawGizmosSelectedMethodInfo(info.ability.GetType())?.Invoke(info.ability, Array.Empty<object>());
                GetOnDrawGizmosSelectedMethodInfo(info.taskDomain.GetType())?.Invoke(info.taskDomain, Array.Empty<object>());
            }
        }

        internal static MethodInfo GetOnDrawGizmosMethodInfo(Type type)
        {
            if (_typeToOnDrawGizmosMethod.TryGetValue(type, out var method))
            {
                return method;
            }
                
            _typeToOnDrawGizmosMethod[type] = type
                .GetMethod(
                    "OnDrawGizmos", 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
            return _typeToOnDrawGizmosMethod[type];
        }
        
        internal static MethodInfo GetOnDrawGizmosSelectedMethodInfo(Type type)
        {
            if (_typeToOnDrawGizmosSelectedMethod.TryGetValue(type, out var method))
            {
                return method;
            }
                
            _typeToOnDrawGizmosSelectedMethod[type] = type
                .GetMethod(
                    "OnDrawGizmosSelected", 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
            return _typeToOnDrawGizmosSelectedMethod[type];
        }
        
#endif
        #endregion

        #region 私有方法

        private void Setup()
        {
            foreach (var abilityInstance in abilities)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    HandleGaveAbility(abilityInstance);
                    continue;
                }
#endif
                
                abilityInstance.ability = Instantiate(abilityInstance.ability);

                HandleGaveAbility(abilityInstance);
            }
            
            foreach (var attributeSet in attributeSets)
            {
                HandleAddedAttributeSet(attributeSet);
            }

            foreach (var effect in effects)
            {
                HandleAddedEffect(effect);
            }
        }

        private void AbilitiesUpdate()
        {
            // 尝试使用触发器激活技能
            foreach (var abilityInstance in _abilitiesBuffer)
            {
                abilityInstance.taskDomain.Update(Time.deltaTime);
                abilityInstance.trigger?.UpdateInternal(Time.deltaTime);
                abilityInstance.ability.TryActivate();
            }
        }

        private void AttributeSetsUpdate()
        {
            foreach (var attributeSet in _attributeSetsBuffer)
            {
                attributeSet.UpdateInternal(Time.deltaTime);
            }
        }

        private void EffectsUpdate()
        {
            foreach (var effect in _effectsBuffer)
            {
                effect.Update(Time.deltaTime);
            }

            foreach (var effect in _effectsBuffer)
            {
                effect.Apply();
            }
        }

        private void HandleGaveAbility(AbilityInstance abilityInstance)
        {
            if (abilityInstance == null) return;
            if (abilityInstance.ability == null) return;
            
            abilityInstance.Init(this);
            abilityInstance.ability.OnGive();
        }

        private void HandleRemovedAbility(AbilityInstance abilityInstance)
        {
            if (abilityInstance.ability.isActivated) abilityInstance.ability.Cancel();
            
            abilityInstance.ability.OnRemove();
        }

        private void HandleAddedAttributeSet(AttributeSet attributeSet)
        {
            attributeSet.Init(this);
        }

        private void HandleRemovedAttributeSet(AttributeSet attributeSet)
        {
        }

        private void HandleAddedEffect(GameplayEffect effect)
        {
            effect.InitInternal(this);
        }
        
        #endregion

        #region Editor

        [Conditional("UNITY_EDITOR")]
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

        [Conditional("UNITY_EDITOR")]
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
        
        #endregion
    }
}