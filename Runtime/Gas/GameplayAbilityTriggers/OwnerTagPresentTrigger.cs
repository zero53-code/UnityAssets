using System;
using System.ComponentModel;
using UnityEngine;
using Zero53.GameplayTags;

namespace Zero53.Gas.GameplayAbilityTriggers
{
    [Serializable]
    [Description("拥有该 Tag 期间持续触发")]
    public class OwnerTagPresentTrigger : GameplayAbilityTrigger
    {
        /// <summary>
        /// 激活技能的标签
        /// </summary>
        [SerializeField]
        private Tag[] tags;
        
        protected internal override void OnUpdate(float deltaTime)
        {
            if (tags.Length == 0 || abilitySystem.tags.HasAny(tags))
            {
                ActivateAbility();
            }
        }
    }
}