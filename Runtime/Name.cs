using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Zero53
{
    [Serializable]
    public struct Name : IEquatable<Name>, IComparable<Name>
    {
        [SerializeField] private string name;
        private int _hashCode;
        private static readonly Dictionary<string, int> _nameId = new();
        private static int _nextId = 1;

        public Name(string name)
        {
            this.name = name ?? string.Empty;
            string.Intern(this.name);
            _hashCode = GetId(this.name);
        }

        public Name(Name other)
        {
            name = other.name;
            _hashCode = other.GetHashCode();
        }

        public static implicit operator string(Name name)
        {
            return name.name;
        }

        public static implicit operator Name(string name)
        {
            return new Name(name);
        }

        public bool isEmpty => string.IsNullOrEmpty(name);

        public override string ToString()
        {
            return name;
        }

        public override int GetHashCode()
        {
            if (_hashCode == 0) _hashCode = GetId(name);

            return _hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is string str) return str == name;

            return obj is Name other && Equals(other);
        }

        public bool Equals(Name other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public int CompareTo(Name other)
        {
            return GetHashCode().CompareTo(other.GetHashCode());
        }

        public static bool operator ==(Name left, Name right) => left.Equals(right);

        public static bool operator !=(Name left, Name right) => !(left == right);

        private static int GetId(string name)
        {
            lock (_nameId)
            {
                if (name == null) return 0;
                if (_nameId.TryGetValue(name, out var result)) return result;

                _nameId[name] = _nextId;
                result = _nextId;
                if (result == 0)
                {
                    throw new OverflowException();
                }

                _nextId++;

                return result;
            }
        }
    }

#if UNITY_EDITOR
    public class AttributeNameDrawer : OdinValueDrawer<Name>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var value = ValueEntry.SmartValue;
            var newValue = SirenixEditorGUI.DynamicPrimitiveField(label, value.ToString());
            ValueEntry.SmartValue = newValue;
        }
    }
#endif
}
