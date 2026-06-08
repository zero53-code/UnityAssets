using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53.Gas
{
    // [InlineEditor]
    public class GameplayAttributeSet : ScriptableObject
    {
        public GameplayAbilitySystem abilitySystem { get; private set; }
        
        internal void Init(GameplayAbilitySystem abilitySystem)
        {
            this.abilitySystem = abilitySystem;

            foreach (var info in GetAttributeDataFields())
            {
                var data = (GameplayAttributeData)info.GetValue(this);
                if (info.GetValue(this) is not null)
                {
                    data.Init(this);
                    continue;
                }

                data = new GameplayAttributeData();
                info.SetValue(this, data);

                data.Init(this);
            }
           
            foreach (var info in GetAttributeDataProperties())
            {
                var data = (GameplayAttributeData)info.GetValue(this);
                if (info.GetValue(this) is not null)
                {
                    data.Init(this);
                    continue;
                }

                data = new GameplayAttributeData();
                info.SetValue(this, data);

                data.Init(this);
            }
        }

        private static readonly Dictionary<Type, FieldInfo[]> _attributeDataFields = new();
        private static readonly Dictionary<Type, PropertyInfo[]> _attributeSetsProperties = new();

        private FieldInfo[] GetAttributeDataFields()
        {
            var type = GetType();
            if (_attributeDataFields.TryGetValue(type, out var result))
            {
                return result;
            }
            
            result = type
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(GameplayAttributeData))
                .ToArray();
            
            _attributeDataFields[type] = result;
            
            return result;
        }
        
        private PropertyInfo[] GetAttributeDataProperties()
        {
            var type = GetType();
            if (_attributeSetsProperties.TryGetValue(type, out var result))
            {
                return result;
            }
            
            result = type
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.PropertyType == typeof(GameplayAttributeData))
                .ToArray();
            
            _attributeSetsProperties[type] = result;
            
            return result;
        }
        
        private readonly List<GameplayEffect> _effectsBuffer = new();
        
        internal void UpdateInternal(float deltaTime)
        {
        }
        
        protected internal virtual void PreAttributeChange(GameplayAttributeData data, ref float newValue)
        {
        }
        
        protected internal virtual void PreAttributeBaseChange(GameplayAttributeData data, ref float newValue)
        {
        }

        protected internal virtual void PreGameplayEffectApply(GameplayEffect effect)
        {
        }
        
        protected internal virtual void PostGameplayEffectApply(GameplayEffect effect)
        {
        }
        
        internal IEnumerable<(string, GameplayAttributeData)> GetAttributeData()
        {
            foreach (var attributeData in GetAttributeDataFields()
                         .Select(f => (f.Name, (GameplayAttributeData)f.GetValue(this))))
            {
                yield return attributeData;
            }
            
            foreach (var attributeData in GetAttributeDataProperties()
                         .Select(f => (f.Name, (GameplayAttributeData)f.GetValue(this))))
            {
                yield return attributeData;
            }
        }

    }

#if UNITY_EDITOR

    public class AttributeSetDrawer : OdinValueDrawer<GameplayAttributeSet>
    {
        private readonly List<(string, GameplayAttributeData)> _list = new();
        private GUITable _table;
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);

            SirenixEditorGUI.BeginBox();
            
            _list.Clear();
            _list.AddRange((Property.ValueEntry.WeakSmartValue as GameplayAttributeSet)
                           ?.GetAttributeData()
                           ?? Array.Empty< (string, GameplayAttributeData)>());
            
            _table ??= GUITable.Create(
                _list,
                label?.text,
                new GUITableColumn
                {
                    ColumnTitle = "Name",
                    OnGUI = (rect, i) => SirenixEditorFields.TextField(rect, _list[i].Item1)
                },
                new GUITableColumn
                {
                    ColumnTitle = "Base Value",
                    OnGUI = (rect, i) =>
                    {
                        _list[i].Item2.baseValue = SirenixEditorFields.FloatField(rect, _list[i].Item2.baseValue);
                    }
                },
                new GUITableColumn
                {
                    ColumnTitle = "Current Value",
                    OnGUI = (rect, i) =>
                    {
                        _list[i].Item2.currentValue = SirenixEditorFields.FloatField(rect, _list[i].Item2.currentValue);
                    }
                });
            
            _table.DrawTable();
        
            SirenixEditorGUI.EndBox();
        }
    }
    
#endif
}