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
        
        protected override void Initialize()
        {
            LoadIcon();
        }

        private void LoadIcon()
        {
            if (_icon != null) return;

            var guid = Attribute.guid;
            var path = Attribute.path;

            // 优先 GUID
            if (!string.IsNullOrEmpty(guid))
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
            }

            if (!string.IsNullOrEmpty(path))
            {
                _icon = AssetDatabase.LoadAssetAtPath<Texture>(path);
            }
        }
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (_icon != null && label != null)
            {
                label.image = _icon;
            }
            
            CallNextDrawer(label);
        }
        
    }
    
#endif
}