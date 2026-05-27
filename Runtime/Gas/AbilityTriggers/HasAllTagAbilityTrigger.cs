using System;
using Zero53.GameplayTags;
using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTriggers
{
    /// <summary>
    /// 有全部 tag 时触发技能
    /// </summary>
    [Serializable]
    public class HasAllTagAbilityTrigger : IAbilityTrigger
    {
        public Tag[] tags;
        
        private Tags _tags;

        public void Init(GameplayAbility ability)
        {
            _tags = ability.abilitySystem.tags;
        }
        
        public bool Check(float deltaTime)
        {
            return _tags.HasAll(tags);
        }
    }
}