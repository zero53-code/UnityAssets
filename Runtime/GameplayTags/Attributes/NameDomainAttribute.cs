// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Reflection;
// using Sirenix.OdinInspector.Editor;
// using Sirenix.Utilities;
// using UnityEngine;
//
// namespace Zero53.GameplayTags.Attributes
// {
//     [Conditional("UNITY_EDITOR")]
//     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field)]
//     public sealed class NameDomainAttribute : Attribute
//     {
//         public string namesGetter;
//
//         public NameDomainAttribute(string namesGetter)
//         {
//             this.namesGetter = namesGetter;
//         }
//     }
//     
// #if UNITY_EDITOR
//
//     public class NameDomainProcessor : OdinDrawer
//     {
//         protected override void DrawPropertyLayout(GUIContent label)
//         {
//             var parent = Property?.Parent;
//             var value = parent?.ValueEntry?.WeakSmartValue;
//             
//             if (value == null)
//             {
//                 CallNextDrawer(label);
//                 return;
//             }
//
//             var nameDomainAttribute = value.GetType().GetAttribute<NameDomainAttribute>();
//             if (nameDomainAttribute == null)
//             {
//                 CallNextDrawer(label);
//                 return;
//             }
//             
//             var getterName = nameDomainAttribute.namesGetter;
//             var sourceNames = GetNameListFromGetter(Property?.ValueEntry?.WeakSmartValue, getterName);
//             if (sourceNames == null)
//             {
//                 CallNextDrawer(label);
//                 return;
//             }
//             
//             // 递归遍历所有子属性，设置 NameDropdown.names
//             ProcessChildrenRecursive(parent.Children, sourceNames);
//             
//             CallNextDrawer(label);
//         }
//
//         /// <summary>
//         /// 反射获取字段/属性/方法返回的 string 列表
//         /// </summary>
//         private static IList<string> GetNameListFromGetter(object target, string getterName)
//         {
//             if (string.IsNullOrEmpty(getterName)) return null;
//
//             if (target == null) return null;
//
//             var type = target.GetType();
//             const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
//
//             // 字段
//             var field = type.GetField(getterName, flags);
//             if (field != null) return field.GetValue(target) as IList<string>;
//
//             // 属性
//             var prop = type.GetProperty(getterName, flags);
//             if (prop != null) return prop.GetValue(target) as IList<string>;
//
//             // 方法
//             var method = type.GetMethod(getterName, flags);
//             if (method != null && method.GetParameters().Length == 0)
//                 return method.Invoke(target, null) as IList<string>;
//
//             return null;
//         }
//
//         /// <summary>
//         /// 递归遍历子属性，给 NameDropdownAttribute 赋值 names
//         /// </summary>
//         private static void ProcessChildrenRecursive(IEnumerable<InspectorProperty> children, IList<string> names)
//         {
//             foreach (var child in children)
//             {
//                 // 找到带 NameDropdownAttribute 的字段
//                 var dropdownAttr = child.GetAttribute<NameDropdownAttribute>();
//                 if (dropdownAttr != null)
//                 {
//                     // 赋值 names
//                     dropdownAttr.names = names;
//                 }
//
//                 // 递归子节点
//                 if (child.Children.Count > 0)
//                     ProcessChildrenRecursive(child.Children, names);
//             }
//         }
//     }
//
// #endif
//     
// }