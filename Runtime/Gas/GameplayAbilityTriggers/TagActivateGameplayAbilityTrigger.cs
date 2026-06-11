using System;
using System.ComponentModel;
using Zero53.GameplayTags;

namespace Zero53.Gas.GameplayTriggers
{
    [Serializable]
    [Description(description: "基于 tag 触发技能")]
    public class TagActivateGameplayAbilityTrigger : GameplayAbilityTrigger
    {
        /// <summary>
        /// 激活技能的标签
        /// </summary>
        public Tag[] activateAbilityTags;
        
        protected internal override void OnUpdate(float deltaTime)
        {
            if (abilitySystem.tags.HasAny(activateAbilityTags))
            {
                ActivateAbility();
            }
        }
    }
}