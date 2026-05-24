using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Zero53.GameplayTags
{
    [Serializable]
    public struct Tag : IEquatable<Tag>, IComparable<Tag>
    {
        /// <summary>
        /// 标签完整名称, 层级使用 . 分隔，例如：Skill.Fire.Damage
        /// </summary>
        [SerializeField]
        private string fullName;

        /// <summary>
        /// 使用标签全名创建 Tag 实例
        /// </summary>
        /// <param name="tagFullName">标签完整路径名称</param>
        public Tag(string tagFullName)
        {
            fullName = tagFullName ?? string.Empty;
            string.Intern(fullName);
        }

        /// <summary>
        /// 标签层级
        /// </summary>
        public int layer => fullName.Split('.').Length;
        
        /// <summary>
        /// 是否为空标签
        /// </summary>
        public bool isEmpty => fullName is null || fullName.Length == 0;
        
        /// <summary>
        /// 是否为有效标签
        /// </summary>
        public bool isValid => !isEmpty;

        /// <summary>
        /// 判断当前标签是否与目标标签匹配（支持父集匹配）
        /// </summary>
        /// <param name="other">目标匹配标签</param>
        /// <returns>是否匹配成功</returns>
        public bool Matches(Tag other)
        {
            if (!isValid || !other.isValid) return false;
            if (fullName == other.fullName) return true;
            
            return fullName.StartsWith(other.fullName + '.');
        }

        /// <summary>
        /// 判断当前标签与目标标签是否完全一致（精确匹配）
        /// </summary>
        /// <param name="other">目标标签</param>
        /// <returns>是否完全相等</returns>
        public bool MatchesExact(Tag other)
        {
            return isValid && other.isValid && this == other;
        }
        
        /// <summary>
        /// 判断当前标签是否是目标标签的直接子标签
        /// </summary>
        /// <param name="other">父级候选标签</param>
        /// <returns>是否是直接子标签</returns>
        public bool IsChildOf(Tag other)
        {
            if (isEmpty) return false;
            if (other.isEmpty) return false;

            var tagUnits = fullName.Split('.');
            var parentTag = string.Join('.', tagUnits.Take(tagUnits.Length - 1));

            return other.fullName == parentTag;
        }

        /// <summary>
        /// 判断当前标签是否是目标标签的直接父标签
        /// 等价于 other.IsChildOf(this)
        /// </summary>
        /// <param name="other">子级候选标签</param>
        /// <returns>是否是直接父标签</returns>
        public bool IsParentOf(Tag other)
        {
            return other.IsChildOf(this);
        }
        
        /// <summary>
        /// 比较两个标签是否相等（基于 fullName）
        /// </summary>
        /// <param name="other">目标标签</param>
        /// <returns>是否相等</returns>
        public bool Equals(Tag other)
        {
            return fullName == other.fullName;
        }
        
        /// <summary>
        /// 重写相等比较
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Tag other && Equals(other);
        }

        /// <summary>
        /// 重写哈希码, 使用标签全名计算
        /// </summary>
        public override int GetHashCode()
        {
            return fullName.GetHashCode();
        }

        /// <summary>
        /// 标签比较规则: 先按名称字符串排序, 再按层级排序
        /// </summary>
        public int CompareTo(Tag other)
        {
            
            var hashCodeComparing = string.Compare(fullName, other.fullName, StringComparison.Ordinal);
            return hashCodeComparing != 0 
                ? hashCodeComparing 
                : layer.CompareTo(other.layer);
        }

        /// <summary>
        /// 相等运算符重载
        /// </summary>
        public static bool operator ==(Tag left, Tag right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 不相等运算符重载
        /// </summary>
        public static bool operator !=(Tag left, Tag right)
        {
            return !(left == right);
        }

        /// <summary>
        /// 字符串隐式转换为 Tag
        /// </summary>
        public static implicit operator Tag(string tag)
        {
            return new Tag(tag);
        }
        
        /// <summary>
        /// Tag 隐式转换为 Name
        /// </summary>
        public static implicit operator Name(Tag tag)
        {
            return new Name(tag.fullName);
        }
        
        /// <summary>
        /// 返回标签的可读字符串
        /// </summary>
        public override string ToString()
        {
            if (isValid) return fullName;
            return "(empty)";
        }
        
        /// <summary>
        /// 获取当前标签的所有祖先标签（从顶层父级到最近父级）
        /// </summary>
        public IEnumerable<Tag> GetParents()
        {
            if (isEmpty) yield break;
            var tagUnits = fullName.Split('.');
            
            if (tagUnits.Length == 1) yield break;

            for (var i = 1; i < layer; i++)
            {
                yield return new Tag(string.Join('.', tagUnits[new Range(0, i)]));
            }
        }

        /// <summary>
        /// 全局标签容器缓存
        /// </summary>
        private static TagContainer _tagLibrary;

        /// <summary>
        /// 全局标签库，自动从 TagLibrary 实例加载所有可用标签
        /// </summary>
        public static TagContainer tagLibrary
        {
            get
            {
                if (_tagLibrary != null) return _tagLibrary;
                
                _tagLibrary = new TagContainer();

                _tagLibrary.Append(TagLibrary.instance.tags.Select(t => new Tag(t)));
                
                return _tagLibrary;
            }
        }
    }
    
#if UNITY_EDITOR

    namespace Editor
    {
        public class TagDrawer : OdinValueDrawer<Tag>
        {
            protected override void DrawPropertyLayout(GUIContent label)
            {
                var tagList = Tag.tagLibrary.ToList();

                // 绘制一个可点击的框
                var rect = EditorGUILayout.GetControlRect();
                if (label != null) rect = EditorGUI.PrefixLabel(rect, label);

                var value = ValueEntry.SmartValue;

                // 显示文本
                string displayText;
                var style = EditorStyles.popup;

                if (value == null)
                {
                    style = new GUIStyle(style);
                    var textColor = style.normal.textColor;
                    textColor.a = 0.5f;
                    style.normal.textColor = textColor;
                    displayText = "Select Tag";
                }
                else
                {
                    displayText = value.ToString();
                }

                GUI.Label(rect, displayText, style);

                if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
                {
                    var selector = new GenericSelector<Tag>("Select Tag", tagList);
                    selector.SelectionConfirmed += selection => ValueEntry.SmartValue = selection.First();
                    selector.ShowInPopup();
                    Event.current.Use();
                }
            }
        }
    }
    
#endif
}