namespace Zero53.Gas
{
    public interface IAbilityEffectData<in TSource, in TTarget>
    {
        IAbilityEffect<TSource, TTarget> CreateAbilityEffect();
    }
}