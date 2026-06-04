using System.ComponentModel;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53.Utils.Attributes
{
#if UNITY_EDITOR

    internal class DescriptionAttributeDrawer : OdinAttributeDrawer<DescriptionAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            SirenixEditorGUI.MessageBox(Attribute.Description);
            
            CallNextDrawer(label);
        }
    }

#endif
}