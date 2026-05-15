using System.Collections.Generic;
using UnityEngine;

namespace Zero53.GameplayTags
{
    [DisallowMultipleComponent]
    public class Tags : MonoBehaviour
    {
        [field: SerializeField]
        public TagContainer tagContainer { get; set; } = new();
        
        public void AddTag(Tag tag)
        {
            tagContainer.AddTag(tag);
        }

        public void RemoveTag(Tag tag)
        {
            tagContainer.RemoveTag(tag);
        }
        
        public void RemoveTagExact(Tag tag)
        {
            tagContainer.RemoveTagExact(tag);
        }

        public void AppendTags(IEnumerable<Tag> other)
        {
            tagContainer.AppendTags(other);
        }

        public void RemoveTags(IEnumerable<Tag> other)
        {
            tagContainer.RemoveTags(other);
        }
        
        public void RemoveTagsExact(IEnumerable<Tag> other)
        {
            tagContainer.RemoveTagsExact(other);
        }

        public void Clear()
        {
            tagContainer.Clear();
        }
        
        public bool HasTag(Tag tag)
        {
            return tagContainer.HasTag(tag);
        }

        public bool HasTagExact(Tag tag)
        {
            return tagContainer.HasTagExact(tag);
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
    }
}