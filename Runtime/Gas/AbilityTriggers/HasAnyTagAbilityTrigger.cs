using System;
using Zero53.GameplayTags;
using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTriggers
{
    /// <summary>
    /// 有任意一个 tag 就触发技能
    /// </summary>
    [Serializable]
    public class HasAnyTagAbilityTrigger : IAbilityTrigger
    {
        public Tag[] tags;
        
        private TagContainer _tags;

        public void Init(GameplayAbility ability)
        {
            _tags = ability.abilitySystem.tags;
        }
        
        public bool Check(float deltaTime)
        {
            return _tags.HasAny(tags);
        }
    }
}