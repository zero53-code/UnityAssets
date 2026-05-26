using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Zero53.Persistence
{
    /// <summary>
    /// 一个可序列化的 GUID
    /// </summary>
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid>, IComparable<SerializableGuid>
    {
        [field: SerializeField] public uint value0 { get; private set; }
        [field: SerializeField] public uint value1 { get; private set; }
        [field: SerializeField] public uint value2 { get; private set; }
        [field: SerializeField] public uint value3 { get; private set; }

        internal SerializableGuid(uint value0, uint value1, uint value2, uint value3)
        {
            this.value0 = value0;
            this.value1 = value1;
            this.value2 = value2;
            this.value3 = value3;
        }

        private static Random _random = new();

        public static SerializableGuid Generate()
        {
            return new SerializableGuid(
                (uint)_random.Next(),
                (uint)_random.Next(),
                (uint)_random.Next(),
                (uint)_random.Next());
        }

        public bool isEmpty => value0 == 0 && value1 == 0 && value2 == 0 && value3 == 0;

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

#if UNITY_EDITOR
    
    internal class SerializableGuidDrawer : OdinValueDrawer<SerializableGuid>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var guid = ValueEntry.SmartValue;

            // 绘制前缀标签（如字段名）
            var totalRect = EditorGUILayout.GetControlRect();
            if (label != null)
                totalRect = EditorGUI.PrefixLabel(totalRect, label);

            // 把一行平均分成 4 份
            var width = totalRect.width / 4f;
            var r0 = new Rect(totalRect.x, totalRect.y, width - 2, totalRect.height);
            var r1 = new Rect(totalRect.x + width, totalRect.y, width - 2, totalRect.height);
            var r2 = new Rect(totalRect.x + width * 2, totalRect.y, width - 2, totalRect.height);
            var r3 = new Rect(totalRect.x + width * 3, totalRect.y, width - 2, totalRect.height);

            // 分别绘制 4 个十六进制输入框
            var v0 = DrawHexField(r0, guid.value0);
            var v1 = DrawHexField(r1, guid.value1);
            var v2 = DrawHexField(r2, guid.value2);
            var v3 = DrawHexField(r3, guid.value3);

            // 赋值回结构体
            ValueEntry.SmartValue = new SerializableGuid(v0, v1, v2, v3);
        }
        
        /// <summary>
        /// 绘制一个 8 位十六进制输入框
        /// </summary>
        private static uint DrawHexField(Rect rect, uint value)
        {
            var hex = value.ToString("X8");
            var input = EditorGUI.TextField(rect, hex);

            return uint.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out var result)
                ? result 
                : value;
        }

        // 把 4个uint 转 32位十六进制
        private static string ConvertToHex(SerializableGuid guid)
        {
            return
                guid.value0.ToString("X8") +
                guid.value1.ToString("X8") +
                guid.value2.ToString("X8") +
                guid.value3.ToString("X8");
        }

        // 解析 32位十六进制 → 4个uint
        private static bool TryParseHex(string hex, out uint v0, out uint v1, out uint v2, out uint v3)
        {
            v0 = v1 = v2 = v3 = 0;

            if (string.IsNullOrEmpty(hex) || hex.Length != 32)
                return false;

            try
            {
                v0 = Convert.ToUInt32(hex[0..8], 16);
                v1 = Convert.ToUInt32(hex[8..16], 16);
                v2 = Convert.ToUInt32(hex[16..24], 16);
                v3 = Convert.ToUInt32(hex[24..32], 16);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    
#endif
}