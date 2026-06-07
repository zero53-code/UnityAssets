using System;
using System.ComponentModel;

namespace Zero53.Gas
{
    [Serializable]
    [Description(description: "技能未激活时触发")]
    public class NonActivatedAbilityTrigger : AbilityTriggerBase
    {
        protected internal override void Update(float deltaTime)
        {
            if (!ability.isActivated)
            {
                ActivateAbility();
            }
        }
    }
}