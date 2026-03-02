#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Zero53.Editor
{
    /// <summary>
    /// 自定义编辑器显示字段名称 Drawer
    /// </summary>
    [CustomPropertyDrawer(typeof(InspectorNameAttribute))]
    public class FieldNameAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label.text = (attribute as InspectorNameAttribute)?.displayName ?? label.text;

            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif