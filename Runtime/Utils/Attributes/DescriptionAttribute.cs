
using System;
using System.Diagnostics;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public sealed class DescriptionAttribute : Attribute
    {
        public readonly string description;
        public DescriptionAttribute(string description)
        {
            this.description = description;
        }
    }

#if UNITY_EDITOR

    internal class DescriptionAttributeDrawer : OdinAttributeDrawer<System.ComponentModel.DescriptionAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            SirenixEditorGUI.MessageBox(Attribute.Description);
            
            CallNextDrawer(label);
        }
    }

    internal class CustomDescriptionAttributeDrawer : OdinAttributeDrawer<DescriptionAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            SirenixEditorGUI.MessageBox(Attribute.description);
            
            CallNextDrawer(label);
        }
    }

#endif
}

