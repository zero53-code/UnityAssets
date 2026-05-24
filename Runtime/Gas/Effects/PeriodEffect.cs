using System;
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.Gas.Attributes;

namespace Zero53.Gas.Effects
{
    /// <summary>
    /// 周期效果
    /// </summary>
    [Serializable]
    public class PeriodEffect : IGameplayEffect
    {
        /// <summary>
        /// 效果
        /// </summary>
        [OdinSerialize, SerializeReference] public IGameplayEffect effect;
        
        /// <summary>
        /// 持续时间
        /// </summary>
        [Min(0f)] public float duration = float.PositiveInfinity;
        
        /// <summary>
        /// 周期时间
        /// </summary>
        [Min(0f)] public float period;
        
        /// <summary>
        /// 是否立刻执行一次
        /// </summary>
        public bool immediatelyOnce;
        
        /// <summary>
        /// 持续时间计时器
        /// </summary>
        [ProgressBar(min: 0, maxGetter: "duration")]
        public float durationTimer;

        /// <summary>
        /// 周期时间计时器
        /// </summary>
        [ProgressBar(min: 0, maxGetter: "period")]
        public float periodTimer;
        
        /// <summary>
        /// 是否处于暂停状态
        /// </summary>
        public bool isPaused;
        
        public void Apply(GameplayAttributeSet target)
        {
            if (immediatelyOnce && durationTimer <= Mathf.Epsilon)
            {
                effect?.Apply(target);
            }
            
            if (durationTimer >= duration)
            {
                target.RemoveEffect(this);
                return;
            }
            
            if (isPaused) return;
            
            durationTimer += Time.deltaTime;
            periodTimer += Time.deltaTime;

            if (periodTimer < period) return;
                
            effect?.Apply(target);
            periodTimer -= period;
        }
    }
}