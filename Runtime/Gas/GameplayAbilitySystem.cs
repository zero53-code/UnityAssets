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
        public event Action<GameplayAbilityBase> PostAbilityGave;
        
        /// <summary>
        /// 移除技能前调用
        /// </summary>
        public event Action<GameplayAbilityBase> PreAbilityRemoved;
        
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

        [OdinSerialize, SerializeField, PropertyOrder(order: 0)]
        [OnCollectionChanged("BeforeAbilitiesChange", "AfterAbilitiesChange")]
        [InlineEditor]
        [LabelIcon(guid: "aac75bf07cb097640819d1d102c8d3b4")]
        private List<GameplayAbilityBase> abilities = new();

        [OdinSerialize, SerializeReference, PropertyOrder(order: 1)] 
        [LabelIcon(guid: "6f14c79b09e2dba418ec247c90766138")]
        private GameplayAttributeSet[] attributeSets;

        [field: SerializeField, PropertyOrder(order: 2)]
        [field: LabelIcon(guid: "d64403f63082071429603d00539686a7")]
        public TagContainer tags { get; private set; }
        
        [SerializeReference, PropertyOrder(order: 3)] 
        [LabelIcon(guid: "e3690992982611f48b01d88a57537d37")]
        private List<GameplayEffect> effects = new();
        
        [SerializeReference, PropertyOrder(order: 4)]
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

        public TAbility GiveAbility<TAbility>(AbilityTrigger trigger) where TAbility : GameplayAbilityBase, new()
        {
            return GiveAbility(trigger, ScriptableObject.CreateInstance<TAbility>());
        }
        
        public TAbility GiveAbilityAndActivateOnce<TAbility>(AbilityTrigger trigger) where TAbility : GameplayAbilityBase, new()
        {
            var ability = GiveAbility<TAbility>(trigger);

            if (ability != null)
            {
                ability.OnCommit();
            }
            
            return ability;
        }
        
        public TAbility GiveAbility<TAbility>(AbilityTrigger trigger, TAbility newAbility) where TAbility : GameplayAbilityBase
        {
            if (newAbility == null) return null;
            
            foreach (var ability in abilities)
            {
                if (ability == newAbility) return null;
            }

            var abilityInstance = ScriptableObject.CreateInstance<TAbility>();
            
            abilities.Add(abilityInstance);
            
            HandleGaveAbility(abilityInstance);
            
            return newAbility;
        }

        public void CancelAbility<TAbility>() where TAbility : GameplayAbilityBase
        {
            foreach (var ability in abilities)
            {
                if (ability is TAbility)
                {
                    ability.Cancel();
                }
            }
        }
        
        public int RemoveAbility<TAbility>() where TAbility : GameplayAbilityBase
        {
            return abilities.RemoveAll(abilityInstance =>
            {
                if (abilityInstance is not TAbility) return false;
                
                HandleRemovedAbility(abilityInstance);
                
                return true;
            });
        }

        public bool RemoveAbility<TAbility>(TAbility ability) where TAbility : GameplayAbilityBase
        {
            var count = abilities.RemoveAll(abilityInstance =>
            {
                if (abilityInstance != ability) return false;
                
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
            foreach (var ability in abilities)
            {
                HandleRemovedAbility(ability);
                Destroy(ability);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var ability in abilities)
            {
                ability.InvokeOnValidate();
                ability.trigger.InvokeOnValidate();
            }

            if (!Application.isPlaying)
            {
                Setup();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var ability in abilities)
            {
                ability.InvokeOnDrawGizmos();
                ability.trigger.InvokeOnDrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var ability in abilities)
            {
                ability.InvokeOnDrawGizmosSelected();
                ability.InvokeOnDrawGizmosSelected();
                ability.trigger.InvokeOnDrawGizmosSelected();
            }
        }
        
#endif
        #endregion

        #region 私有字段
        
        private readonly List<GameplayAbilityBase> _abilitiesBuffer  = new();
        private readonly List<GameplayAttributeSet> _attributeSetsBuffer = new();
        private readonly List<PeriodicGameplayEffect> _periodicEffects = new();
        private readonly List<PeriodicGameplayEffect> _periodicEffectsBuffer = new();
        private readonly List<GameplayCue> _cuesBuffer = new();

        #endregion

        #region 私有方法

        private void Setup()
        {
            for (var i = 0; i < abilities.Count; i++)
            {
                abilities[i] = abilities[i].InstantiatePlayModeOnly();

                HandleGaveAbility(abilities[i]);
            }

            for (var i = 0; i < attributeSets.Length; i++)
            {
                attributeSets[i] = attributeSets[i].InstantiatePlayModeOnly();
                
                HandleAddedAttributeSet(attributeSets[i]);
            }

            for (var i = 0; i < effects.Count; i++)
            {
                effects[i] = effects[i].InstantiatePlayModeOnly();
                
                HandleAddedEffect(effects[i]);
            }
        }

        private void AbilitiesUpdate()
        {
            // 尝试使用触发器激活技能
            foreach (var ability in _abilitiesBuffer)
            {
                ability.UpdateInternal(Time.deltaTime);
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
        private void HandleGaveAbility(GameplayAbilityBase ability)
        {
            if (ability == null) return;
            
            ability.InitInternal(this);
            ability.OnGive();
            PostAbilityGave?.Invoke(ability);
        }

        /// <summary>
        /// 处理已移除的技能
        /// </summary>
        private void HandleRemovedAbility(GameplayAbilityBase ability)
        {
            if (ability == null) return;
            
            if (ability.isActivated) ability.Cancel();
            
            PreAbilityRemoved?.Invoke(ability);
            ability.RemoveInternal();
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

            if (effect is PeriodicGameplayEffect periodEffect)
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
            if (effect is PeriodicGameplayEffect periodEffect)
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
                    abilities[info.Index].Cancel();
                    break;
                
                case CollectionChangeType.Clear:
                    abilities.ForEach(ability => ability.Cancel());
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