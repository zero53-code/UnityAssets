using System;
using System.ComponentModel;
using Zero53.GameplayTags;

namespace Zero53.Gas.AbilityTriggers
{
    [Serializable]
    [Description(description: "基于 tag 触发或禁用技能")]
    public class TagAbilityTrigger : AbilityTriggerBase
    {
        /// <summary>
        /// 激活技能的标签
        /// </summary>
        public Tag[] activateAbilityTags;
        
        /// <summary>
        /// 禁用技能的标签
        /// </summary>
        public Tag[] deactivateAbilityTags;
        
        protected internal override void OnUpdate(float deltaTime)
        {
            if (activateAbilityTags == null || activateAbilityTags.Length == 0) return;

            if (deactivateAbilityTags != null && 
                deactivateAbilityTags.Length != 0 && 
                abilitySystem.tags.HasAny(deactivateAbilityTags))
            {
                return;
            }

            if (abilitySystem.tags.HasAny(activateAbilityTags))
            {
                ActivateAbility();
            }
        }
    }
}