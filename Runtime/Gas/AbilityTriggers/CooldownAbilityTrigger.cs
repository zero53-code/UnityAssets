using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTriggers
{
    /// <summary>
    /// 冷却时间技能触发器
    /// </summary>
    [Serializable]
    public class CooldownAbilityTrigger : IAbilityTrigger
    {
        /// <summary>
        /// 冷却持续时间
        /// </summary>
        [Min(0f), HorizontalGroup] 
        public float duration = 1f;
        
        /// <summary>
        /// 冷却计时器
        /// </summary>
        [Min(0f), HorizontalGroup, ProgressBar(min: 0f, maxGetter: "duration")] 
        public float timer;
        
        private GameplayAbility _ability;

        public void Init(GameplayAbility ability)
        {
            _ability = ability;
        }
        
        public bool Check(float deltaTime)
        {
            if (_ability.isExecuting)
            {
                timer = 0f;
                return false;
            }
            
            timer += deltaTime;
            return timer >= duration;
        }
    }
}