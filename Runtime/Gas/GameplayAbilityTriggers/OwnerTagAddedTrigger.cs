using System;
using System.ComponentModel;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Zero53.GameplayTags;

namespace Zero53.Gas.GameplayAbilityTriggers
{
    [Serializable]
    [Description("加上指定 Tag 时触发一次")]
    public class OwnerTagAddedTrigger : GameplayAbilityTrigger
    {
        [SerializeField]
        private Tag[] tags;
        
        protected internal override void OnInit()
        {
            owner.tags.OnTagAdded += OnTagAdded;
        }

        protected internal override void OnUpdate(float deltaTime)
        {
            if (tags.Length != 0 && !_canActivate) return;
            
            _canActivate = false;
            ActivateAbility();
        }

        private bool _canActivate;
        private void OnTagAdded(Tag newTag)
        {
            if (tags.Any(tag => newTag.Matches(tag)))
            {
                _canActivate = true;
            }
        }

        protected internal override void OnRemove()
        {
            owner.tags.OnTagAdded -= OnTagAdded;
        }
    }
    
#if UNITY_EDITOR
    
    internal class OwnerTagAddedTriggerDrawer : OdinValueDrawer<OwnerTagAddedTrigger>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Property.Children["tags"].Draw(label);
        }
    }
    
#endif
}