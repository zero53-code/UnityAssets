using System;
using UnityEngine;

namespace Zero53.Gas.AbilityTriggers
{
    [Serializable]
    public class AndAbilityTrigger : IAbilityTrigger
    {
        [SerializeReference] public IAbilityTrigger[] triggers = {};
        
        public bool Check(float deltaTime)
        {
            var result = true;
            foreach (var trigger in triggers)
            {
                result &= trigger.Check(deltaTime);
            }
            
            return result;
        }
    }
}