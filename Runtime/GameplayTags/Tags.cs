using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zero53.GameplayTags
{
    [DisallowMultipleComponent]
    public class Tags : MonoBehaviour, IEnumerable<Tag>
    {
        [SerializeField]
        private TagContainer tagContainer = new();

        public void RegisterAddTagEvent(Action<Tag> onAddTag)
        {
            tagContainer.OnAddTag += onAddTag;
        }

        public void RegisterRemoveTagEvent(Action<Tag> onRemoveTag)
        {
            tagContainer.OnRemoveTag += onRemoveTag;
        }

        public void UnregisterAddTagEvent(Action<Tag> onAddTag)
        {
            tagContainer.OnAddTag -= onAddTag;
        }

        public void UnregisterRemoveTagEvent(Action<Tag> onRemoveTag)
        {
            tagContainer.OnRemoveTag -= onRemoveTag;
        }
        
        public void AddTag(Tag tag)
        {
            tagContainer.Add(tag);
        }

        public void RemoveTag(Tag tag)
        {
            tagContainer.Remove(tag);
        }
        
        public void RemoveTagExact(Tag tag)
        {
            tagContainer.RemoveExact(tag);
        }

        public void AppendTags(IEnumerable<Tag> other)
        {
            tagContainer.Append(other);
        }

        public void RemoveTags(IEnumerable<Tag> other)
        {
            tagContainer.Remove(other);
        }
        
        public void RemoveTagsExact(IEnumerable<Tag> other)
        {
            tagContainer.RemoveExact(other);
        }

        public void Clear()
        {
            tagContainer.Clear();
        }
        
        public bool HasTag(Tag tag)
        {
            return tagContainer.Has(tag);
        }

        public bool HasTagExact(Tag tag)
        {
            return tagContainer.HasExact(tag);
        }

        public bool HasAny(IEnumerable<Tag> other)
        {
            return tagContainer.HasAny(other);
        }

        public bool HasAll(IEnumerable<Tag> other)
        {
            return tagContainer.HasAll(other);
        }

        public bool HasAnyExact(IEnumerable<Tag> other)
        {
            return tagContainer.HasAnyExact(other);
        }

        public bool HasAllExact(IEnumerable<Tag> other)
        {
            return tagContainer.HasAllExact(other);
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return tagContainer.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)tagContainer).GetEnumerator();
        }
    }
}