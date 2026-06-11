using System;
using System.ComponentModel;
using Zero53.GameplayTags;

namespace Zero53.Gas.GameplayTriggers
{
    [Serializable]
    [Description(description: "基于 tag 触发或禁用技能")]
    public class TagGameplayAbilityTrigger : GameplayAbilityTrigger
    {
        public bool defaultActivate;
        
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
            if (abilitySystem.tags.HasAny(deactivateAbilityTags))
            {
                return;
            }

            if (defaultActivate)
            {
                if (activateAbilityTags == null || activateAbilityTags.Length == 0)
                {
                    ActivateAbility();
                    return;
                }
            }

            if (abilitySystem.tags.HasAny(activateAbilityTags))
            {
                ActivateAbility();
            }
        }
    }
}