using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTriggers
{
    public interface IAbilityTrigger
    {
        void Init(GameplayAbility ability) {}
        bool Check(float deltaTime);
    }
}