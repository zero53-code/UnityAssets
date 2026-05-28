using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.AttributeSet.Processor;

namespace Zero53.Gas.AttributeSet
{
    [CreateAssetMenu(menuName = "Zero53/Gas/Gameplay Attribute Set", fileName = "New Gameplay Attribute Set")]
    [NameDomain("_namesGetter")]
    public class GameplayAttributeSetAsset : ScriptableObject
    {
        [field: SerializeField]
        public string attributeSetName = "";
        
        [field: SerializeField]
        public List<AttributeInfo> attributes { get; private set; } = new();

        [Serializable]
        public class AttributeInfo
        {
            /// <summary>
            /// 属性名
            /// </summary>
            [HorizontalGroup(GroupName = "1", Width = 0.25f, MarginRight = 20)] [LabelWidth(40)]
            public string name;

            /// <summary>
            /// 属性展示名
            /// </summary>
            [HorizontalGroup(GroupName = "1")] [LabelWidth(90)] 
            public string displayName;
            
            /// <summary>
            /// 基础值
            /// </summary>
            public float value;
            
            /// <summary>
            /// 属性描述
            /// </summary>
            [TextArea] [HideLabel] 
            public string description;
            

            [SerializeReference]
            public IChangeProcessor[] changeProcessors = {};
        }

        private void OnValidate()
        {
            if (!GameplayAttributeSet.IsValidName(attributeSetName))
            {
                Debug.LogWarning($"Attribute set name '{attributeSetName}' is invalid.");
                attributeSetName = name;
            }

            foreach (var info in attributes)
            {
                if (GameplayAttributeSet.IsValidName(info.name)) continue;
                
                Debug.LogWarning($"Attribute set name '{info.name}' is invalid.");
                info.name = "attribute_name";
            }
        }

#if UNITY_EDITOR
        
        private IList<string> _namesGetter => attributes.Select(info => info.name).ToList();
        
#endif
        
    }
}