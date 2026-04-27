using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Zero53.Persistence
{
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid>, IComparable<SerializableGuid>
    {
        [SerializeField] private uint value0;
        [SerializeField] private uint value1;
        [SerializeField] private uint value2;
        [SerializeField] private uint value3;

        private SerializableGuid(uint value0, uint value1, uint value2, uint value3)
        {
            this.value0 = value0;
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }

        public static SerializableGuid Generate()
        {
            return new SerializableGuid(
                (uint)Random.Range(int.MinValue, int.MaxValue),
                (uint)Random.Range(int.MinValue, int.MaxValue),
                (uint)Random.Range(int.MinValue, int.MaxValue),
                (uint)Random.Range(int.MinValue, int.MaxValue));
        }

        public bool Equals(SerializableGuid other)
        {
            return value0 == other.value0 && value1 == other.value1 && value2 == other.value2 && value3 == other.value3;
        }

        public override bool Equals(object obj)
        {
            return obj is SerializableGuid other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(value0, value1, value2, value3);
        }

        public override string ToString()
        {
            return value0.ToString("X8") + value1.ToString("X8") + value2.ToString("X8") + value3.ToString("X8");
        }

        public int CompareTo(SerializableGuid other)
        {
            var value0Comparison = value0.CompareTo(other.value0);
            if (value0Comparison != 0) return value0Comparison;
            var value1Comparison = value1.CompareTo(other.value1);
            if (value1Comparison != 0) return value1Comparison;
            var value2Comparison = value2.CompareTo(other.value2);
            if (value2Comparison != 0) return value2Comparison;
            return value3.CompareTo(other.value3);
        }

        public static bool operator ==(SerializableGuid x, SerializableGuid y) => Equals(x, y);

        public static bool operator !=(SerializableGuid x, SerializableGuid y) => !(x == y);
    }
}