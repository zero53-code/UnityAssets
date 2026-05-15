using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53.GameplayTags
{
    [Serializable]
    public struct GameplayTag : IEquatable<GameplayTag>, IComparable<GameplayTag>
    {
        [SerializeField]
        private string fullName;

        public GameplayTag(string tagFullName)
        {
            fullName = tagFullName ?? string.Empty;
            string.Intern(fullName);
        }

        public int layer => fullName.Split('.').Length;
        public bool isEmpty => fullName is null || fullName.Length == 0;
        public bool isValid => !isEmpty;

        public bool Matches(GameplayTag other)
        {
            if (!isValid || !other.isValid) return false;
            if (fullName == other.fullName) return true;
            
            return fullName.StartsWith(other.fullName + '.');
        }

        public bool MatchesExact(GameplayTag other)
        {
            return isValid && other.isValid && this == other;
        }
        
        public bool IsChildOf(GameplayTag other)
        {
            if (isEmpty) return false;
            if (other.isEmpty) return false;

            var tagUnits = fullName.Split('.');
            var parentTag = string.Join('.', tagUnits.Take(tagUnits.Length - 1));

            return other.fullName == parentTag;
        }

        public bool IsParentOf(GameplayTag other)
        {
            return other.IsChildOf(this);
        }
        
        public bool Equals(GameplayTag other)
        {
            return fullName == other.fullName;
        }
        
        public override bool Equals(object obj)
        {
            return obj is GameplayTag other && Equals(other);
        }

        public override int GetHashCode()
        {
            return fullName.GetHashCode();
        }

        public int CompareTo(GameplayTag other)
        {
            
            var hashCodeComparing = string.Compare(fullName, other.fullName, StringComparison.Ordinal);
            return hashCodeComparing != 0 
                ? hashCodeComparing 
                : layer.CompareTo(other.layer);
        }

        public static bool operator ==(GameplayTag left, GameplayTag right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GameplayTag left, GameplayTag right)
        {
            return !(left == right);
        }

        public static implicit operator GameplayTag(string tag)
        {
            return new GameplayTag(tag);
        }
        
        public static implicit operator Name(GameplayTag tag)
        {
            return new Name(tag.fullName);
        }
        
        public override string ToString()
        {
            if (isValid) return fullName;
            return "(empty)";
        }
        
        public IEnumerable<GameplayTag> GetParents()
        {
            if (isEmpty) yield break;
            var tagUnits = fullName.Split('.');
            
            if (tagUnits.Length == 1) yield break;

            for (var i = 1; i < layer; i++)
            {
                yield return new GameplayTag(string.Join('.', tagUnits[new Range(0, i)]));
            }
        }
    }
    
#if UNITY_EDITOR
    public class GameplayTagDrawer : OdinValueDrawer<GameplayTag>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            List<GameplayTag> tagList = GameplayTagLibrary.instance.tags.Select(t => new GameplayTag(t)).ToList();
            
            // if (GameplayTagLibrary.isInitialized)
            //     tagList = GameplayTagLibrary.instance.tags.Select(t => new GameplayTag(t)).ToList();
            // else
            //     tagList = new List<GameplayTag>();

            
            SirenixEditorFields.Dropdown(label, ValueEntry.SmartValue, tagList);
        }
    }
#endif
}