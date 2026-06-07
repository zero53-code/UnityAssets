using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Zero53.Gas.Effects;

namespace Zero53.Gas.AttributeSets
{
    [Serializable]
    public class AttributeSet
    {
        public AbilitySystem abilitySystem { get; private set; }

        [SerializeReference]
        private List<GameplayEffect> effects;

        public void AddEffect(GameplayEffect effect)
        {
            effects.Add(effect);
            effect.InitInternal(abilitySystem);
        }
        
        public void AddEffects(IEnumerable<GameplayEffect> effects)
        {
            var gameplayEffects = effects as GameplayEffect[] ?? effects.ToArray();
            this.effects.AddRange(gameplayEffects);
            foreach (var effect in gameplayEffects)
            {
                effect.InitInternal(abilitySystem);
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
        
        internal void Init(AbilitySystem abilitySystem)
        {
            this.abilitySystem = abilitySystem;
            
            foreach (var attributeData in GetType()
                         .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         .Where(f => f.FieldType == typeof(AttributeData))
                         .Select(f =>
                         {
                             var data = f.GetValue(this);
                             if (f.GetValue(this) is not null) return (AttributeData)data;
                             
                             data = new AttributeData();
                             f.SetValue(this, data);
                             return (AttributeData)data;
                         }))
            {
                attributeData.Init(this);
            }
            
            foreach (var attributeData in GetType()
                         .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                         .Where(f => f.PropertyType == typeof(AttributeData))
                         .Select(f =>
                         {
                             var data = f.GetValue(this);
                             if (f.GetValue(this) is not null) return (AttributeData)data;
                             
                             data = new AttributeData();
                             f.SetValue(this, data);
                             return (AttributeData)data;
                         }))
            {
                attributeData.Init(this);
            }
        }
        
        private readonly List<GameplayEffect> _effectsBuffer = new();
        internal void UpdateInternal(float deltaTime)
        {
            _effectsBuffer.Clear();
            _effectsBuffer.AddRange(effects);
            
            foreach (var effect in _effectsBuffer)
            {
                effect.Update(deltaTime);
            }

            foreach (var effect in _effectsBuffer)
            {
                PreGameplayEffectApply(effect);
                effect.Apply();
                PostGameplayEffectApply(effect);
            }
            
            Update(deltaTime);
        }
        
        protected virtual void Update(float deltaTime) {}

        protected internal virtual void PreAttributeChange(AttributeData data, ref float newValue)
        {
        }
        
        protected internal virtual void PreAttributeBaseChange(AttributeData data, ref float newValue)
        {
        }

        protected internal virtual void PreGameplayEffectApply(GameplayEffect effect)
        {
        }
        
        protected internal virtual void PostGameplayEffectApply(GameplayEffect effect)
        {
        }
    }

#if UNITY_EDITOR

    public class AttributeSetDrawer : OdinValueDrawer<AttributeSet>
    {
        private readonly List<InspectorProperty> _attributeDataProperty = new();
        private readonly List<AttributeData> _list = new();
        private GUITable _table;
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            _attributeDataProperty.Clear();
            
            foreach (var property in Property.Children)
            {
                if (property.ValueEntry.TypeOfValue != typeof(AttributeData))
                {
                    property.Draw();
                }
                else
                {
                    _attributeDataProperty.Add(property);
                }
            }
            
            if (_attributeDataProperty.Count == 0) return;

            _list.Clear();
            _list.AddRange(_attributeDataProperty
                .Select(p => p.ValueEntry.WeakSmartValue as AttributeData));

            _table ??= GUITable.Create(
                _list,
                label?.text,
                new GUITableColumn
                {
                    ColumnTitle = "Name",
                    OnGUI = (rect, i) => EditorGUI.TextField(rect, _attributeDataProperty[i].Label.text)
                },
                new GUITableColumn
                {
                    ColumnTitle = "Base Value",
                    OnGUI = (rect, i) => { _list[i]._baseValue = EditorGUI.FloatField(rect, _list[i]._baseValue); }
                },
                new GUITableColumn
                {
                    ColumnTitle = "Current Value",
                    OnGUI = (rect, i) => { _list[i]._currentValue = EditorGUI.FloatField(rect, _list[i]._currentValue); }
                });

            _table.DrawTable();
        }
    }
    
#endif
}