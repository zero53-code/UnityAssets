using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero53
{
    public class GameplayAttributeSet : MonoBehaviour
    {
        [SerializeField] private GameplayAttributeSetData[] data;

        [ShowInInspector, ReadOnly] private Dictionary<Name, GameplayAttribute> _attributes;

        private void Awake()
        {
            Setup();
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

        [Button]
        private void Setup()
        {
            _attributes ??= new Dictionary<Name, GameplayAttribute>();
            if (this.data == null) return;
            
#if UNITY_EDITOR
            GameplayAttributeSetData[] data;
            if (Application.isPlaying)
            {
                data = new GameplayAttributeSetData[this.data.Length];
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

        private void AddAttributeSet(GameplayAttributeSetData attributeSetData)
        {
            if (attributeSetData == null) return;
            
            foreach (var info in attributeSetData.attributes)
            {
                var attributeName = info.name;
                var gameplayAttribute = new GameplayAttribute(this, info.value);
                gameplayAttribute.preAttributeChangeList.AddRange(info.preAttributeChangeList);
                
                if (!_attributes.TryAdd(attributeName, gameplayAttribute))
                {
                    throw new Exception($"Character attribute {attributeName} already exists");
                }
            }
        }
    }
}