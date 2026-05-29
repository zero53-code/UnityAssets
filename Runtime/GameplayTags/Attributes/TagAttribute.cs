// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Sirenix.OdinInspector.Editor;
// using Sirenix.Utilities.Editor;
// using UnityEngine;
//
// namespace Zero53.GameplayTags.Attributes
// {
//     [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
//     public sealed class TagAttribute : Attribute
//     {
//     }
//
// #if UNITY_EDITOR
//
//     public class TagAttributeDrawer : OdinAttributeDrawer<TagAttribute>
//     {
//         protected override void DrawPropertyLayout(GUIContent label)
//         {
//             Name currentTag;
//             
//             if (Property.BaseValueEntry.WeakSmartValue is Name n)
//             {
//                 currentTag = n;
//             }
//             else if (Property.BaseValueEntry.WeakSmartValue is string s)
//             {
//                 currentTag = s;
//             }
//             else
//             {
//                 Property.Draw(label);
//                 return;
//             }
//
//             var tagList = TagLibrary.instance != null 
//                 ? TagLibrary.instance.tags.Select(t => (Name)t).ToList() 
//                 : new List<Name>();
//
//             SirenixEditorFields.Dropdown(label, currentTag, tagList);
//         }
//     }
//     
// #endif
// }