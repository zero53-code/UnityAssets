using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.Attributes;

namespace Zero53.Gas.Effects
{
    /// <summary>
    /// 周期效果
    /// </summary>
    [Serializable]
    public abstract class PeriodEffect : IGameplayEffect
    {
        /// <summary>
        /// 持续时间
        /// </summary>
        [Min(0.0001f)] public float duration = float.PositiveInfinity;
        
        /// <summary>
        /// 周期时间
        /// </summary>
        [Min(0.0001f)] public float period = 1f;
        
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
                OnApply(target);
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
                
            OnApply(target);
            periodTimer -= period;
        }
        
        protected abstract void OnApply(GameplayAttributeSet target);
    }
}