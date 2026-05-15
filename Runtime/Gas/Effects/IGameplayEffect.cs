using Zero53.Gas.Attributes;

namespace Zero53.Gas.Effects
{
    public interface IGameplayEffect
    {
        void Apply(GameplayAttributeSet target);
    }
}