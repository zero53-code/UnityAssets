using System;
using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTriggers
{
    [Serializable]
    public abstract class AbilityTrigger
    {
        protected internal GameplayAbility ability;
        
        protected internal virtual void Init() {}
        protected internal abstract bool Check(float deltaTime);
    }
}