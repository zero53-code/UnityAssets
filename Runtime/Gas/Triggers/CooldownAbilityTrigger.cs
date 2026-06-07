using System;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero53.Gas.Triggers
{
    [Serializable]
    [Description(description: "冷却时间结束时触发")]
    public class CooldownAbilityTrigger : AbilityTriggerBase
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
        
        protected internal override void Update(float deltaTime)
        {
            // 技能激活时不计算冷却时间
            if (ability.isActivated)
            {
                timer = 0;
                return;
            }
            
            if (timer < duration) timer += deltaTime;
            else ActivateAbility();
        }
    }
}