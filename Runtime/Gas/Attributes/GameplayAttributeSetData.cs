using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero53
{
    [CreateAssetMenu(menuName = "Zero53/Gas/Gameplay Attribute Set", fileName = "New Gameplay Attribute Set")]
    public class GameplayAttributeSetData : ScriptableObject
    {
        [field: SerializeField]
        public List<AttributeInfo> attributes { get; private set; } = new();

        private void OnValidate()
        {
            NameDropdownAttribute.items = attributes
                .Select(item => new Name(item.name))
                .Where(n => !n.isEmpty)
                .ToList();
        }

        [Serializable]
        public class AttributeInfo
        {
            [HorizontalGroup(GroupName = "1", Width = 0.25f, MarginRight = 20)] [LabelWidth(40)]
            public string name;

            [HorizontalGroup(GroupName = "1")] [LabelWidth(90)] 
            public string displayName;
            
            public float value;
            
            [TextArea] [HideLabel] 
            public string description;
            

            [SerializeReference]
            public IPreAttributeChange[] preAttributeChangeList = {};
        }
    }
}