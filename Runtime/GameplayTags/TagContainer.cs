using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Zero53.GameplayTags
{
    [Serializable]
    public class TagContainer : IEnumerable<Tag>
    {
        public event Action<Tag> OnAddTag;
        public event Action<Tag> OnRemoveTag;
        
        [SerializeField]
        private List<Tag> tags;

        private SortedSet<Tag> _parentTags = new();

        public TagContainer()
        {
            tags = new List<Tag>();
        }

        public TagContainer(params Tag[] tags)
        {
            this.tags = new List<Tag>(tags);
        }

        public bool isEmpty => tags.Count == 0;
        public int count => tags.Count;
        
        #region 增删

        public bool Add(Tag tag)
        {
            if (!tag.isValid || tags.Contains(tag)) return false;
            
            tags.Add(tag);
            OnAddTag?.Invoke(tag);
            
            foreach (var parent in tag.GetParents())
            {
                _parentTags.Add(parent);
            }
            return true;
        }

        public int Remove(Tag tag)
        {
            var result = tags.RemoveAll(t =>
            {
                if (!t.Matches(tag)) return false;
                
                OnRemoveTag?.Invoke(tag);
                return true;
            });
            
            if (result != 0) FillParent();
            
            return result;
        }
        
        public bool RemoveExact(Tag tag)
        {
            if (tags.RemoveAll(t => t.MatchesExact(tag)) == 0) return false;
            
            OnRemoveTag?.Invoke(tag);
            FillParent();
            return true;
        }

        public int Append(IEnumerable<Tag> other)
        {
            var result = 0;
            foreach (var t in other)
            {
                if (Add(t)) result++;
            }
            
            return result;
        }

        public int Remove(IEnumerable<Tag> other)
        {
            var result = 0;
            foreach (var tag in other)
            {
                result += tags.RemoveAll(t =>
                {
                    if (!t.Matches(tag)) return false;
                
                    OnRemoveTag?.Invoke(tag);
                    return true;
                });
            }
            
            FillParent();
            return result;
        }
        
        public int RemoveExact(IEnumerable<Tag> other)
        {
            var result = 0;
            foreach (var tag in other)
            {
                result += tags.RemoveAll(t =>
                {
                    if (!t.MatchesExact(tag)) return false;
                
                    OnRemoveTag?.Invoke(tag);
                    return true;
                });
            }
            
            FillParent();
            return result;
        }

        public void Clear()
        {
            tags.Clear();
            _parentTags.Clear();
        }

        private void FillParent()
        {
            _parentTags.Clear();
            foreach (var tag in tags)
            {
                foreach (var parentTag in tag.GetParents())
                {
                    _parentTags.Add(parentTag);
                }
            }
        }

        #endregion

        #region 查询匹配

        public bool Has(Tag tag)
        {
            return _parentTags.Contains(tag) || tags.Contains(tag);
        }

        public bool HasExact(Tag tag)
        {
            return tags.Contains(tag);
        }

        public bool HasAny(IEnumerable<Tag> other)
        {
            if (other == null) return true;
            return other.Any(Has);
        }

        public bool HasAll(IEnumerable<Tag> other)
        {
            if (other == null) return true;
            return other.All(Has);
        }

        public bool HasAnyExact(IEnumerable<Tag> other)
        {
            if (other == null) return true;
            return other.Any(HasExact);
        }

        public bool HasAllExact(IEnumerable<Tag> other)
        {
            if (other == null) return true;
            return other.All(HasExact);
        }

        public IEnumerable<Tag> Query(Func<Tag, bool> condition)
        {
            return tags.Where(condition);
        }
        
        #endregion

        #region 集合运算

        // 交集
        public TagContainer Filter(TagContainer other)
        {
            var res = new TagContainer();
            foreach (var t in tags)
            {
                if (other.Has(t)) res.Add(t);
            }

            return res;
        }

        // 差集：自身有，对方没有
        public TagContainer Difference(TagContainer other)
        {
            var res = new TagContainer();
            foreach (var t in tags)
            {
                if (!other.Has(t)) res.Add(t);
            }

            return res;
        }

        #endregion

        public IEnumerator<Tag> GetEnumerator()
        {
            return tags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<Tag> GetParents()
        {
            foreach (var parentTag in _parentTags)
            {
                yield return parentTag;
            }

            foreach (var tag in tags)
            {
                yield return tag;
            }
        }

        public IEnumerable<Tag> GetChildren(Tag parent)
        {
            return tags.Where(t => t.Matches(parent));
        }
    }
    
#if UNITY_EDITOR
    public class GameplayTagContainerDrawer : OdinValueDrawer<TagContainer>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var slotsProperty = Property.Children["tags"];

            slotsProperty.Draw(label);
        }
    }
#endif
}