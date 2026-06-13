using System;
using System.ComponentModel;

namespace Zero53.Gas.GameplayAbilityTriggers
{
    [Serializable]
    [Description(description: "技能未激活时触发")]
    public class NonActivatedGameplayAbilityTrigger : GameplayAbilityTrigger
    {
        protected internal override void OnUpdate(float deltaTime)
        {
            if (!ability.isActivated)
            {
                ActivateAbility();
            }
        }
    }
}