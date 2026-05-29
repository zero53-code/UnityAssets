using System;
using System.Collections.Generic;
using UnityEngine;
using Zero53.GameplayTags;

namespace Zero53.GameplayFactions
{
    public class FactionSetting : ScriptableObject
    {
        public Tag factionParentTag;
        
        [SerializeField]
        private List<HostileList> a;
        
        [Serializable]
        private struct HostileList
        {
            public List<Hostile> list;
        }
        
        [Serializable]
        private struct Hostile
        {
            public Tag factionTag;
            public bool isHostile;
        }

        private void OnValidate()
        {
            a ??= new List<HostileList>();

            var factionTags = Tag.tagLibrary.GetChildren(factionParentTag);
            
        }
    }
}