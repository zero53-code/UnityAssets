using System;
using UnityEngine;

namespace Zero53.Gas.AbilityTriggers
{
    [Serializable]
    public class NotAbilityTrigger : IAbilityTrigger
    {
        [SerializeReference] public IAbilityTrigger trigger;
        
        public bool Check(float deltaTime)
        {
            return !trigger.Check(deltaTime);
        }
    }
}