using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Zero53.Gas.Effects;

namespace Zero53.Gas.Attributes
{
    public class GameplayAttributeSet : MonoBehaviour
    {
        [SerializeField, TableList] private List<GameplayAttributeData> attributes = new();

        [SerializeReference] private List<IGameplayEffect> effects = new();
        
        private readonly Dictionary<Name, GameplayAttributeData> _nameToAttribute = new();

        #region Unity 生命周期

        private void Awake()
        {
            Setup();
        }

        private void Update()
        {
            ApplyEffects();
        }

        #endregion
        
        #region API

        public float this[Name attributeName]
        {
            get => _nameToAttribute[attributeName].value;
            set => _nameToAttribute[attributeName].SetValue(this, value);
        }

        public bool TryGetAttribute(Name attributeName, out float attributeValue)
        {
            if (_nameToAttribute.TryGetValue(attributeName, out var attributeData))
            {
                attributeValue = attributeData.value;
                return true;
            }

            attributeValue = 0;
            return false;
        }
        
        public float GetAttribute(Name attributeName)
        {
            return this[attributeName];
        }

        public float GetAttribute(Name attributeName, float defaultValue)
        {
            return TryGetAttribute(attributeName, out var attributeValue) 
                ? attributeValue 
                : defaultValue;
        }

        public void SetAttribute(Name attributeName, float attributeValue)
        {
            this[attributeName] = attributeValue;
        }

        public float GetAttributeBaseValue(Name attributeName)
        {
            return _nameToAttribute[attributeName].baseValue;
        }

        public float SetAttributeBaseValue(Name attributeName, float baseValue)
        {
            return _nameToAttribute[attributeName].baseValue = baseValue;
        }

        public bool HasAttribute(Name attributeName)
        {
            return _nameToAttribute.ContainsKey(attributeName);
        }

        public bool AddAttribute(Name attributeName, float baseValue, float initialValue)
        {
            if (HasAttribute(attributeName)) return false;
            
            var data = new GameplayAttributeData(attributeName, baseValue, initialValue);
            attributes.Add(data);
            _nameToAttribute[attributeName] = data;
            
            return true;
        }

        public bool AddAttribute(Name attributeName, float baseValue) => AddAttribute(attributeName, baseValue, baseValue);

        public bool RemoveAttribute(Name attributeName)
        {
            if (!HasAttribute(attributeName)) return false;

            var data = _nameToAttribute[attributeName];
            attributes.Remove(data);
            _nameToAttribute.Remove(attributeName);
            
            return true;
        }
        
        public void AddAttributeSetAsset(GameplayAttributeSetAsset asset)
        {
            if (asset == null) return;
            
            foreach (var info in asset.attributes)
            {
                var attributeName = info.name;
                var attribute = new GameplayAttributeData(attributeName, info.value);
                attribute.changeProcessors.AddRange(info.changeProcessors);
                
                attributes.Add(attribute);
                if (!_nameToAttribute.TryAdd(attributeName, attribute))
                {
                    throw new Exception($"Character attribute {attributeName} already exists");
                }
            }
        }

#if UNITY_EDITOR
        
        [Button("AddAttributeSetAsset")]
        private void AddAttributeSetAsset()
        {
            // 找到项目中所有 SO
            var guids = AssetDatabase.FindAssets($"t:{typeof(GameplayAttributeSetAsset).FullName}");
            var list = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameplayAttributeSetAsset>)
                .ToList();

            // 弹出 Odin 选择窗口
            var selector = new GenericSelector<GameplayAttributeSetAsset>("Select Asset", list);
            selector.EnableSingleClickToSelect(); // 单击选择

            selector.SelectionConfirmed += selection => AddAttributeSetAsset(selection.FirstOrDefault());
            selector.ShowInPopup();
        }
        
#endif

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

        #endregion

        private void Setup()
        {
            foreach (var attribute in attributes)
            {
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
                var attribute = new GameplayAttributeData(attributeName, info.value);
                attribute.changeProcessors.AddRange(info.changeProcessors);
                
                attributes.Add(attribute);
                if (!_nameToAttribute.TryAdd(attributeName, attribute))
                {
                    throw new Exception($"Character attribute {attributeName} already exists");
                }
            }
        }

        private readonly List<IGameplayEffect> _effectsBuffer = new();
        private void ApplyEffects()
        {
            _effectsBuffer.Clear();
            _effectsBuffer.AddRange(effects);
            foreach (var effect in _effectsBuffer)
            {
                effect.Apply(this);
            }
        }
    }
}