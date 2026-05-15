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
        [SerializeField]
        private List<Tag> tags;

        // private List<GameplayTag> _parentTags;
        // private SortedList<GameplayTag, GameplayTag> _parentTags = new();
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

        public void AddTag(Tag tag)
        {
            if (!tag.isValid || tags.Contains(tag)) return;
            tags.Add(tag);
            foreach (var parent in tag.GetParents())
            {
                _parentTags.Add(parent);
            }
        }

        public void RemoveTag(Tag tag)
        {
            if (tags.RemoveAll(t => t.Matches(tag)) == 0) return;
            
            FillParentTags();
        }
        
        public void RemoveTagExact(Tag tag)
        {
            if (tags.RemoveAll(t => t.MatchesExact(tag)) == 0) return;
            
            FillParentTags();
        }

        public void AppendTags(IEnumerable<Tag> other)
        {
            foreach (var t in other)
            {
                AddTag(t);
            }
        }

        public void RemoveTags(IEnumerable<Tag> other)
        {
            foreach (var tag in other)
            {
                tags.RemoveAll(t => t.Matches(tag));
            }
            
            FillParentTags();
        }
        
        public void RemoveTagsExact(IEnumerable<Tag> other)
        {
            foreach (var tag in other)
            {
                tags.RemoveAll(t => t.MatchesExact(tag));
            }
            
            FillParentTags();
        }

        public void Clear()
        {
            tags.Clear();
            _parentTags.Clear();
        }

        private void FillParentTags()
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

        public bool HasTag(Tag tag)
        {
            return _parentTags.Contains(tag) || tags.Contains(tag);
        }

        public bool HasTagExact(Tag tag)
        {
            return tags.Contains(tag);
        }

        public bool HasAny(IEnumerable<Tag> other)
        {
            return other.Any(HasTag);
        }

        public bool HasAll(IEnumerable<Tag> other)
        {
            return other.All(HasTag);
        }

        public bool HasAnyExact(IEnumerable<Tag> other)
        {
            return other.Any(HasTagExact);
        }

        public bool HasAllExact(IEnumerable<Tag> other)
        {
            return other.All(HasTagExact);
        }

        #endregion

        #region 集合运算

        // 交集
        public TagContainer Filter(TagContainer other)
        {
            var res = new TagContainer();
            foreach (var t in tags)
            {
                if (other.HasTag(t)) res.AddTag(t);
            }

            return res;
        }

        // 差集：自身有，对方没有
        public TagContainer Difference(TagContainer other)
        {
            var res = new TagContainer();
            foreach (var t in tags)
            {
                if (!other.HasTag(t)) res.AddTag(t);
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