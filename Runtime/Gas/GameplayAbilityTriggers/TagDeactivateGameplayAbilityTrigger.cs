using System;
using System.ComponentModel;
using Zero53.GameplayTags;

namespace Zero53.Gas.GameplayTriggers
{
    [Serializable]
    [Description(description: "基于 tag 禁用技能")]
    public class TagDeactivateGameplayAbilityTrigger : GameplayAbilityTrigger
    {
        /// <summary>
        /// 禁用技能的标签
        /// </summary>
        public Tag[] deactivateAbilityTags;
        
        protected internal override void OnUpdate(float deltaTime)
        {
            if (!abilitySystem.tags.HasAny(deactivateAbilityTags))
            {
                ActivateAbility();
            }
        }
    }
}