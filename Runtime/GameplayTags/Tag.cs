using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53.GameplayTags
{
    [Serializable]
    public struct Tag : IEquatable<Tag>, IComparable<Tag>
    {
        [SerializeField]
        private string fullName;

        public Tag(string tagFullName)
        {
            fullName = tagFullName ?? string.Empty;
            string.Intern(fullName);
        }

        public int layer => fullName.Split('.').Length;
        public bool isEmpty => fullName is null || fullName.Length == 0;
        public bool isValid => !isEmpty;

        public bool Matches(Tag other)
        {
            if (!isValid || !other.isValid) return false;
            if (fullName == other.fullName) return true;
            
            return fullName.StartsWith(other.fullName + '.');
        }

        public bool MatchesExact(Tag other)
        {
            return isValid && other.isValid && this == other;
        }
        
        public bool IsChildOf(Tag other)
        {
            if (isEmpty) return false;
            if (other.isEmpty) return false;

            var tagUnits = fullName.Split('.');
            var parentTag = string.Join('.', tagUnits.Take(tagUnits.Length - 1));

            return other.fullName == parentTag;
        }

        public bool IsParentOf(Tag other)
        {
            return other.IsChildOf(this);
        }
        
        public bool Equals(Tag other)
        {
            return fullName == other.fullName;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Tag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return fullName.GetHashCode();
        }

        public int CompareTo(Tag other)
        {
            
            var hashCodeComparing = string.Compare(fullName, other.fullName, StringComparison.Ordinal);
            return hashCodeComparing != 0 
                ? hashCodeComparing 
                : layer.CompareTo(other.layer);
        }

        public static bool operator ==(Tag left, Tag right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Tag left, Tag right)
        {
            return !(left == right);
        }

        public static implicit operator Tag(string tag)
        {
            return new Tag(tag);
        }
        
        public static implicit operator Name(Tag tag)
        {
            return new Name(tag.fullName);
        }
        
        public override string ToString()
        {
            if (isValid) return fullName;
            return "(empty)";
        }
        
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

        private static TagContainer _tagLibrary;

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
    public class TagDrawer : OdinValueDrawer<Tag>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var tagList = Tag.tagLibrary.ToList();
            
            ValueEntry.SmartValue = SirenixEditorFields.Dropdown(label, ValueEntry.SmartValue, tagList);
        }
    }
#endif
}