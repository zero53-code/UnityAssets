using System;
using System.ComponentModel;

namespace Zero53.Gas.Triggers
{
    [Serializable]
    [Description(description: "一直激活")]
    public class AlwaysActivateAbilityTrigger : AbilityTriggerBase
    {
        protected internal override void Update(float deltaTime)
        {
            ActivateAbility();
        }
    }
}