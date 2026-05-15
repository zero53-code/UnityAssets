using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53.Gas.Attributes
{
    /// <summary>
    /// 用于绘制属性名的下拉框
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class NameDropdownAttribute : Attribute
    {
        public static IList<Name> items = new List<Name>();
    }

#if UNITY_EDITOR
    
    public class NameDropdownAttributeDrawer : OdinAttributeDrawer<NameDropdownAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Name name;
            switch (Property.ValueEntry.WeakSmartValue)
            {
                case Name n:
                    name = n;
                    break;
                case string s:
                    name = s;
                    break;
                default:
                    Property.Draw(label);
                    return;
            }

            Property.ValueEntry.WeakSmartValue = SirenixEditorFields.Dropdown(label, name, NameDropdownAttribute.items);
        }
    }
    
#endif
}