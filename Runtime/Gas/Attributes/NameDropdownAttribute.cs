using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53
{
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
            if (Property.BaseValueEntry.WeakSmartValue is Name name)
            {
                SirenixEditorFields.Dropdown(label, name, NameDropdownAttribute.items);
            }
            else if (Property.BaseValueEntry.WeakSmartValue is string nameString)
            {
                SirenixEditorFields.Dropdown(label, nameString, NameDropdownAttribute.items);
            }
            else
            {
                Property.Draw(label);
            }
        }
    }
    
#endif
}