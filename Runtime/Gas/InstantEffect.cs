using System;

namespace Zero53.Gas
{
    /// <summary>
    /// 即时效果
    /// </summary>
    [Serializable]
    public abstract class InstantEffect : GameplayEffect
    {
        protected internal override void Apply()
        {
            abilitySystem.RemoveEffect(this);
        }
    }
}