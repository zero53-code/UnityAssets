namespace Zero53.Gas.Effects
{
    public interface IGameplayEffect
    {
        /// <summary>
        /// 应用效果 <para/>
        /// 会在每帧执行, 直到效果被移除
        /// </summary>
        /// <param name="target">目标的属性集</param>
        /// <param name="deltaTime"></param>
        void Apply(AbilitySystem target, float deltaTime);
    }
}