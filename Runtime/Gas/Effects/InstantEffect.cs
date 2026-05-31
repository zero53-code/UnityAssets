using System;
using Zero53.Gas.AttributeSets;

namespace Zero53.Gas.Effects
{
    /// <summary>
    /// 即时效果
    /// </summary>
    [Serializable]
    public abstract class InstantEffect : IGameplayEffect
    {
        public void Apply(AbilitySystem target, float deltaTime)
        {
            OnApply(target);
            target.RemoveEffect(this);
        }
        
        /// <summary>
        /// 应用效果 <para/>
        /// 只会执行一次, 然后自动移除效果
        /// </summary>
        /// <param name="target">目标的属性集</param>
        protected abstract void OnApply(AbilitySystem target);
    }
}