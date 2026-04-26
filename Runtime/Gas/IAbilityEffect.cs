namespace Zero53.Gas
{
    public interface IAbilityEffect<in TSource, in TTarget>
    {
        void Apply(TSource source, TTarget target);
    }
}