using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
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
        
        [Button, HorizontalGroup("1", order: 1)]
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
        
        [Button, HorizontalGroup("1", order: 1)]
        private void Remove()
        {
            var tag = new Tag(_tag);
            
            tags.Remove(_tag);
            tags.RemoveAll(t => new Tag(t).Matches(tag));
            
            _tag = "";
            OnValidate();
        }
        
        private void OnValidate()
        {
            for (var i = 0; i < tags.Count; i++)
            {
                var tag = tags[i];
                tags[i] = tag.Replace(" ", "");
            }
            
            tags.RemoveAll(string.IsNullOrWhiteSpace);
            
            RemoveDuplicates();
            Sort();
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
        
        [field: SerializeField, PropertyOrder(order: 2f), DisplayAsString]
        public List<string> tags { get; private set; }

    }
}