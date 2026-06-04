using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Zero53.Singletons;

namespace Zero53.GameplayTags
{
    [CreateAssetMenu(menuName = "Zero53/Create GameplayTagLibrary", fileName = "New GameplayTagLibrary")]
    public class TagLibrary : SOSingleton<TagLibrary>
    {
#if UNITY_EDITOR
        
        [ShowInInspector, PropertyOrder(order: 0f)]
        private string _tag;
        
        [Button, HorizontalGroup("Button", order: 1)]
        private void Add()
        {
            tags.Add(_tag);
            var tag = new Tag(_tag);
            foreach (var t in tag
                         .GetParents()
                         .Where(tagParent => tagParent.isValid))
            {
                tags.Add(t.ToString());
            }
            
            _tag = "";
            
            OnValidate();
        }
        
        [Button, HorizontalGroup("Button", order: 1)]
        private void Delete()
        {
            Delete(_tag);
        }

        private void Delete(string tagString)
        {
            var tag = new Tag(tagString);

            var deletedTags = tags.Where(t => new Tag(t).Matches(tag)).ToArray();
            var msg = $"Are you sure you want to delete these {deletedTags.Length} tags?\n"
                      + string.Join("\n", deletedTags);
            
            if (!EditorUtility.DisplayDialog("Delete Tags", msg, "OK", "Cancel"))
            {
                _tag = "";
                return;
            }
            
            tags.RemoveAll(t => deletedTags.Contains(t));

            _tag = "";
            OnValidate();
        }
        
        private void OnValidate()
        {
            tags.RemoveAll(t => !new Tag(t).isValid);
            
            RemoveDuplicates();
            Sort();
            Tag.tagLibraryInstance = null;
        }
        
        /// <summary>
        /// 去重
        /// </summary>
        private void RemoveDuplicates()
        {
            var tagSet = tags.ToHashSet();
            tags.Clear();
            tags.AddRange(tagSet);
        }
        
        /// <summary>
        /// 排序
        /// </summary>
        private void Sort()
        {
            tags.Sort((tag1, tag2) =>
            {
                var compare = string.Compare(tag1, tag2, StringComparison.Ordinal);
                if (compare != 0) return compare;

                var len1 = tag1.Split('.').Length;
                var len2 = tag2.Split('.').Length;
                return len1.CompareTo(len2);
            });
        }

#endif
        
        [field: SerializeField, PropertyOrder(order: 2f), DisplayAsString, ReadOnly]
        public List<string> tags { get; private set; }
    }

#if UNITY_EDITOR
    
    /// <summary>
    /// 在 Project Setting 中绘制 TagLibrary
    /// </summary>
    internal static class TagLibrarySettingProvider
    {
        private static TagLibrary _tagLibrary;
        private static PropertyTree _propertyTree;
        
        [SettingsProvider]
        public static SettingsProvider CreateMyPluginSettingsProvider()
        {
            var provider = new SettingsProvider("Project/Tag Library", SettingsScope.Project)
            {
                label = "Tag Library",
                guiHandler = _ =>
                {
                    var newTagLibrary = TagLibrary.instance;
                    
                    if (newTagLibrary != _tagLibrary)
                    {
                        _tagLibrary = newTagLibrary;
                        _propertyTree?.Dispose();
                        _propertyTree = PropertyTree.Create(_tagLibrary);
                    }

                    _propertyTree.UpdateTree();
                    _propertyTree.Draw();

                    if (GUI.changed)
                    {
                        EditorUtility.SetDirty(_tagLibrary);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }
            };
            return provider;
        }
    }
    
#endif
}