using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53.GameplayTags
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class GameplayTagAttribute : Attribute
    {
    }

#if UNITY_EDITOR

    public class GameplayTagAttributeDrawer : OdinAttributeDrawer<GameplayTagAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Name currentTag;
            
            if (Property.BaseValueEntry.WeakSmartValue is Name n)
            {
                currentTag = n;
            }
            else if (Property.BaseValueEntry.WeakSmartValue is string s)
            {
                currentTag = s;
            }
            else
            {
                Property.Draw(label);
                return;
            }
            
            List<Name> tagList;
            
            if (GameplayTagLibrary.isInitialized)
                tagList = GameplayTagLibrary.instance.tags.Select(t => (Name)t).ToList();
            else
                tagList = new List<Name>();

            SirenixEditorFields.Dropdown(label, currentTag, tagList);
        }
    }
    
#endif
}