using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.GameplayTags;
using Zero53.Gas.GameplayAbilityTriggers;
using Zero53.Gas.GameplayEffects;
using Zero53.Utils;
using Zero53.Utils.Attributes;

namespace Zero53.Gas
{
    /// <summary>
    /// 游戏技能系统的核心组件
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GameplayAbilitySystem : MonoBehaviour
    {
        #region Events

        /// <summary>
        /// 获得技能后调用
        /// </summary>
        public event Action<GameplayAbility> PostAbilityGave;
        
        /// <summary>
        /// 移除技能前调用
        /// </summary>
        public event Action<GameplayAbility> PreAbilityRemoved;
        
        /// <summary>
        /// 添加效果后调用
        /// </summary>
        public event Action<GameplayEffect> PostEffectAdded;
        
        /// <summary>
        /// 移除效果前调用
        /// </summary>
        public event Action<GameplayEffect> PreEffectRemoved;

        #endregion
        
        #region 序列化

        [OdinSerialize, SerializeField]
        [OnCollectionChanged("BeforeAbilitiesChange", "AfterAbilitiesChange")]
        [LabelIcon(guid: "aac75bf07cb097640819d1d102c8d3b4")]
        private List<GameplayAbilityInstance> abilities = new();

        [OdinSerialize, SerializeReference] 
        [LabelIcon(guid: "6f14c79b09e2dba418ec247c90766138")]
        private GameplayAttributeSet[] attributeSets;

        [field: SerializeField]
        [field: LabelIcon(guid: "d64403f63082071429603d00539686a7")]
        public TagContainer tags { get; private set; }
        
        [SerializeReference, PropertyOrder(order: 1)] 
        [LabelIcon(guid: "e3690992982611f48b01d88a57537d37")]
        private List<GameplayEffect> effects = new();
        
        [SerializeReference, PropertyOrder(order: 2)]
        private List<GameplayCue> cues = new();
        
        #endregion

        #region Abilities API

        public TAttributeSet GetAttributeSet<TAttributeSet>() where TAttributeSet : GameplayAttributeSet
        {
            foreach (var attributeSet in attributeSets)
            {
                if (attributeSet is TAttributeSet set) return set;
            }
            return null;
        }

        public TAttributeSet[] GetAttributeSets<TAttributeSet>() where TAttributeSet : GameplayAttributeSet
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
                ability.OnCommit();
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

            var abilityInstance = new GameplayAbilityInstance(trigger, newAbility);
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

        public bool ApplyEffect(GameplayEffect effect)
        {
            if (effects.Contains(effect)) return false;
            
            HandleAddedEffect(effect);

            if (effect is InstantGameplayEffect)
            {
                HandleRemovedEffect(effect);
            }
            else
            {
                effects.Add(effect);
            }
            
            return true;
        }

        public int ApplyEffects(IEnumerable<GameplayEffect> effects)
        {
            var count = 0;
            foreach (var effect in effects)
            {
                if (ApplyEffect(effect))
                {
                    count++;
                }
            }
            
            return count;
        }

        public bool RemoveEffect(GameplayEffect effect)
        {
            if (!effects.Remove(effect)) return false;
            
            HandleRemovedEffect(effect);
            return true;
        }

        public void ClearEffects()
        {
            foreach (var effect in effects)
            {
                HandleRemovedEffect(effect);
            }
            effects.Clear();
        }

        #endregion

        #region Cues API

        public bool AddCue(GameplayCue cue)
        {
            if (cues.Contains(cue)) return false;
            
            cues.Add(cue);
            HandleAddedCue(cue);
            return true;
        }

        public bool RemoveCue(GameplayCue cue)
        {
            if (!cues.Remove(cue)) return false;
            
            HandleRemovedCue(cue);

            return true;
        }

        #endregion

        #region Unity 生命周期

        private void Awake()
        {
            Setup();
        }
        
        private void Update()
        {
            _abilitiesBuffer.Clear();
            _abilitiesBuffer.AddRange(abilities);
            
            _attributeSetsBuffer.Clear();
            _attributeSetsBuffer.AddRange(attributeSets);
            
            _periodicEffectsBuffer.Clear();
            _periodicEffectsBuffer.AddRange(_periodicEffects);
            
            _cuesBuffer.Clear();
            _cuesBuffer.AddRange(cues);
            
            AttributeSetsUpdate();
            EffectsUpdate();
            AbilitiesUpdate();
            CuesUpdate();
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
            foreach (var info in abilities)
            {
                info.ability.InvokeOnValidate();
                info.taskDomain.InvokeOnValidate();
                info.trigger.InvokeOnValidate();
            }
            
            Setup();
        }

        private void OnDrawGizmos()
        {
            foreach (var info in abilities)
            {
                info.ability.InvokeOnDrawGizmos();
                info.taskDomain.InvokeOnDrawGizmos();
                info.trigger.InvokeOnDrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var info in abilities)
            {
                info.ability.InvokeOnDrawGizmosSelected();
                info.taskDomain.InvokeOnDrawGizmosSelected();
                info.trigger.InvokeOnDrawGizmosSelected();
            }
        }
        
#endif
        #endregion

        #region 私有字段
        
        private readonly List<GameplayAbilityInstance> _abilitiesBuffer  = new();
        private readonly List<GameplayAttributeSet> _attributeSetsBuffer = new();
        private readonly List<GameplayPeriodicEffect> _periodicEffects = new();
        private readonly List<GameplayPeriodicEffect> _periodicEffectsBuffer = new();
        private readonly List<GameplayCue> _cuesBuffer = new();

        #endregion

        #region 私有方法

        private void Setup()
        {
            foreach (var abilityInstance in abilities)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    abilityInstance.ability = Instantiate(abilityInstance.ability);
                }
#else

                abilityInstance.ability = Instantiate(abilityInstance.ability);

#endif
                HandleGaveAbility(abilityInstance);
            }

            for (var i = 0; i < attributeSets.Length; i++)
            {
                var attributeSet = attributeSets[i];

#if UNITY_EDITOR
                if (Application.isPlaying)
                    attributeSets[i] = Instantiate(attributeSet);
#endif
                
                HandleAddedAttributeSet(attributeSets[i]);
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
                abilityInstance.taskDomain.UpdateInternal(Time.deltaTime);
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
            foreach (var periodicEffect in _periodicEffectsBuffer)
            {
                periodicEffect.Update(Time.deltaTime);
            }
        }

        private void CuesUpdate()
        {
            foreach (var cue in _cuesBuffer)
            {
                cue.OnUpdate(Time.deltaTime);
            }
        }
        
        /// <summary>
        /// 处理已获取的技能
        /// </summary>
        private void HandleGaveAbility(GameplayAbilityInstance abilityInstance)
        {
            if (abilityInstance == null) return;
            if (abilityInstance.ability == null) return;
            
            abilityInstance.Init(this);
            abilityInstance.ability.OnGive();
            PostAbilityGave?.Invoke(abilityInstance.ability);
        }

        /// <summary>
        /// 处理已移除的技能
        /// </summary>
        private void HandleRemovedAbility(GameplayAbilityInstance abilityInstance)
        {
            if (abilityInstance == null) return;
            
            if (abilityInstance.ability.isActivated) abilityInstance.ability.Cancel();
            
            PreAbilityRemoved?.Invoke(abilityInstance.ability);
            abilityInstance.ability.OnRemove();
        }

        /// <summary>
        /// 处理已添加的属性集
        /// </summary>
        private void HandleAddedAttributeSet(GameplayAttributeSet attributeSet)
        {
            if (attributeSet == null) return;
            
            attributeSet.InitInternal(this);
        }

        /// <summary>
        /// 处理已移除的属性集
        /// </summary>
        private void HandleRemovedAttributeSet(GameplayAttributeSet attributeSet)
        {
            if (attributeSet == null) return;
        }

        /// <summary>
        /// 处理已添加的效果
        /// </summary>
        private void HandleAddedEffect(GameplayEffect effect)
        {
            if (effect == null) return;
            
            effect.InitInternal(this);

            if (effect is GameplayPeriodicEffect periodEffect)
            {
                _periodicEffects.Add(periodEffect);
            }
            else
            {
                effect.OnApply();
                PostEffectAdded?.Invoke(effect);
            }
        }
        
        /// <summary>
        /// 处理已移除的效果
        /// </summary>
        private void HandleRemovedEffect(GameplayEffect effect)
        {
            if (effect == null) return;
            
            PreEffectRemoved?.Invoke(effect);
            effect.OnRemove();
            if (effect is GameplayPeriodicEffect periodEffect)
            {
                _periodicEffects.Remove(periodEffect);
            }
        }

        private void HandleAddedCue(GameplayCue cue)
        {
            if (cue == null) return;
            
            cue.InitInternal(this);
            cue.OnStart();
        }

        private void HandleRemovedCue(GameplayCue cue)
        {
            if (cue == null) return;
            
            cue.OnRemove();
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