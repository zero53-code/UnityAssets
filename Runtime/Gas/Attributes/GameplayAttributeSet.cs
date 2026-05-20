using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.Effects;

namespace Zero53.Gas.Attributes
{
    public class GameplayAttributeSet : MonoBehaviour
    {
        [SerializeField] private GameplayAttributeSetAsset[] assets = {};
        
        [SerializeField, ReadOnly, HideInInspector] private List<GameplayAttribute> attributes = new();

        [SerializeReference] private List<IGameplayEffect> effects = new();
        
        [ShowInInspector, ReadOnly] private Dictionary<Name, GameplayAttribute> _nameToAttribute;

        #region Unity 生命周期

        private void Awake()
        {
            Setup();
        }

        private void Update()
        {
            ApplyEffects();
        }

        private void OnValidate()
        {
            Setup();
        }
        

        #endregion
        
        #region API

        public GameplayAttribute this[Name gaName] => _nameToAttribute[gaName];
        
        public GameplayAttribute this[string gaName] => _nameToAttribute[gaName];

        public bool TryGetAttribute(Name gaName, out GameplayAttribute attribute)
        {
            if (_nameToAttribute.TryGetValue(gaName, out var attributeInner))
            {
                attribute = attributeInner;
                return true;
            }

            attribute = null;
            return false;
        }

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
        
        [Button]
        private void Setup()
        {
            if (assets == null) return;
            
            _nameToAttribute?.Clear();
            _nameToAttribute ??= new Dictionary<Name, GameplayAttribute>();

            attributes?.Clear();
            attributes ??= new List<GameplayAttribute>();
            
            foreach (var attributeSetAsset in assets)
            {
                AddAttributeSet(attributeSetAsset);
            }
            
            foreach (var attribute in attributes)
            {
                attribute.attributeSet = this;
                
                if (!_nameToAttribute.TryAdd(attribute.name, attribute))
                {
                    throw new Exception($"Character attribute {attribute.name} already exists");
                }
            }
        }

        private void AddAttributeSet(GameplayAttributeSetAsset attributeSetAsset)
        {
            if (attributeSetAsset == null) return;
            
            foreach (var info in attributeSetAsset.attributes)
            {
                var attributeName = info.name;
                var attribute = new GameplayAttribute(attributeName, info.value);
                attribute.changeProcessors.AddRange(info.changeProcessors);
                
                attributes.Add(attribute);
                if (!_nameToAttribute.TryAdd(attributeName, attribute))
                {
                    throw new Exception($"Character attribute {attributeName} already exists");
                }
            }
        }

        private List<IGameplayEffect> _effectsBuffer;
        private void ApplyEffects()
        {
            _effectsBuffer?.Clear();
            _effectsBuffer ??= new List<IGameplayEffect>();
            _effectsBuffer.AddRange(effects);
            foreach (var effect in _effectsBuffer)
            {
                effect.Apply(this);
            }
        }
    }
}