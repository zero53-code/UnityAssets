using System;
using System.Diagnostics;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Zero53.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public sealed class LabelIconAttribute : Attribute
    {
        public readonly string guid;
        public readonly string path;

        public LabelIconAttribute(string guid = null, string path = null)
        {
            this.guid = guid;
            this.path = path;
        }
    }
    
#if UNITY_EDITOR
    
    public class LabelIconAttributeDrawer : OdinAttributeDrawer<LabelIconAttribute>
    {
        private Texture _icon;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (_icon == null)
            {
                var guid = Attribute.guid;
                var path = Attribute.path;

                if (!string.IsNullOrEmpty(guid))
                {
                    path = AssetDatabase.GUIDToAssetPath(guid);
                }

                _icon = AssetDatabase.LoadAssetAtPath<Texture>(path);
            }
            
            if (_icon == null)
            {
                CallNextDrawer(label);
                return;
            }

            if (label != null) label.image = _icon;

            CallNextDrawer(label);
        }
    }
    
#endif
}