using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Zero53.Gas.AttributeSets
{
    [Serializable]
    public class AttributeSet
    {
        // [ShowInInspector, TableList]
        // private List<AttributeData> _attributeData => ReflectionUtils.GetSerializedFields(this)
        //     .Where(info => info.FieldType == typeof(AttributeData))
        //     .Select(info => (AttributeData)info.GetValue(this))
        //     .ToList();
        
        internal void Update()
        {
        }

        #region Attributes API
        
        public AbilitySystem abilitySystem { get; internal set; }

        #endregion
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
                    OnGUI = (rect, i) => { _list[i].baseValue = EditorGUI.FloatField(rect, _list[i].baseValue); }
                },
                new GUITableColumn
                {
                    ColumnTitle = "Current Value",
                    OnGUI = (rect, i) => { _list[i].currentValue = EditorGUI.FloatField(rect, _list[i].currentValue); }
                });

            _table.DrawTable();
        }
    }
    
#endif
}