using Zero53.Gas.Attributes;

namespace Zero53.Gas.Effects
{
    public interface IGameplayEffect
    {
        /// <summary>
        /// 应用效果 <para/>
        /// 会在每帧执行, 直到效果被移除
        /// </summary>
        /// <param name="target">目标的属性集</param>
        void Apply(GameplayAttributeSet target);
    }
}