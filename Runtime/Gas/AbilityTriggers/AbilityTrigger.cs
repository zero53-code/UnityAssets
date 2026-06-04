using System;
using UnityEngine;

namespace Zero53.Gas.AbilityTriggers
{
    [Serializable]
    public sealed class AbilityTrigger : AbilityTriggerBase
    {
        [SerializeReference]
        private AbilityTriggerBase[] triggers;

        protected internal override void OnInit()
        {
            triggers ??= Array.Empty<AbilityTriggerBase>();

            foreach (var trigger in triggers)
            {
                trigger.ability = ability;
                trigger.OnInit();
            }
        }

        protected internal override void OnUpdate(float deltaTime)
        {
            if (triggers == null || triggers.Length == 0) return;
            
            
            foreach (var trigger in triggers)
            {
                trigger.Update(deltaTime);
            }

            var canActivate = true;
            foreach (var trigger in triggers)
            {
                if (trigger.isActive) continue;
                canActivate = false;
                break;
            }

            if (canActivate) ActivateAbility();
        }
    }
}