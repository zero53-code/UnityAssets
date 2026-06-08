using System;
using System.ComponentModel;

namespace Zero53.Gas.GameplayTriggers
{
    [Serializable]
    [Description(description: "一直激活")]
    public class AlwaysActivateGameplayAbilityTrigger : GameplayAbilityTrigger
    {
        protected internal override void Update(float deltaTime)
        {
            ActivateAbility();
        }
    }
}