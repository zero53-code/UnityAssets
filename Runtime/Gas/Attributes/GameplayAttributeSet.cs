using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.Effects;

namespace Zero53.Gas.Attributes
{
    public class GameplayAttributeSet : MonoBehaviour
    {
        [SerializeField] private GameplayAttributeSetAsset[] data;

        [ShowInInspector, ReadOnly] private Dictionary<Name, GameplayAttribute> _attributes;
        
        [SerializeReference] private List<IGameplayEffect> effects = new();

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

        public GameplayAttribute this[Name gaName] => _attributes[gaName];
        
        public GameplayAttribute this[string gaName] => _attributes[gaName];

        public bool TryGetAttribute(Name gaName, out GameplayAttribute attribute)
        {
            if (_attributes.TryGetValue(gaName, out var attributeInner))
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

        public bool RemoveEffect(IGameplayEffect effect)
        {
            return effects.Remove(effect);
        }
        
        
        [Button]
        private void Setup()
        {
            _attributes ??= new Dictionary<Name, GameplayAttribute>();
            if (this.data == null) return;
            
#if UNITY_EDITOR
            GameplayAttributeSetAsset[] data;
            if (Application.isPlaying)
            {
                data = new GameplayAttributeSetAsset[this.data.Length];
                for (var i = 0; i < data.Length; i++)
                {
                    data[i] = Instantiate(this.data[i]);
                }
            }
            else
            {
                data = this.data;
            }
            
#else
            var data = this.data;
#endif
            _attributes?.Clear();
            _attributes ??= new Dictionary<Name, GameplayAttribute>();

            foreach (var attributesData in data)
            {
                AddAttributeSet(attributesData);
            }
        }

        private void AddAttributeSet(GameplayAttributeSetAsset attributeSetAsset)
        {
            if (attributeSetAsset == null) return;
            
            foreach (var info in attributeSetAsset.attributes)
            {
                var attributeName = info.name;
                var gameplayAttribute = new GameplayAttribute(this, info.value);
                gameplayAttribute.changeProcessors.AddRange(info.changeProcessors);
                
                if (!_attributes.TryAdd(attributeName, gameplayAttribute))
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