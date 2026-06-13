using System;
using System.ComponentModel;

namespace Zero53.Gas.GameplayAbilityTriggers
{
    [Serializable]
    [Description(description: "一直激活")]
    public class AlwaysActivateGameplayAbilityTrigger : GameplayAbilityTrigger
    {
        protected internal override void OnUpdate(float deltaTime)
        {
            ActivateAbility();
        }
    }
}