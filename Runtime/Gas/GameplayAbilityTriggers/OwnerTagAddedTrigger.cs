using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            abilitySystem.tags.OnTagAdded += OnTagAdded;
        }

        protected internal override void OnUpdate(float deltaTime)
        {
            if (!_canActivate) return;
            
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
            abilitySystem.tags.OnTagAdded -= OnTagAdded;
        }
    }
}