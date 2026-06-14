using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero53.Gas.GameplayEffects
{
    /// <summary>
    /// 周期效果
    /// </summary>
    public abstract class PeriodicGameplayEffect : GameplayEffect
    {
        /// <summary>
        /// 持续时间
        /// </summary>
        [Min(0.0001f)] public float duration = float.PositiveInfinity;
        
        /// <summary>
        /// 周期时间
        /// </summary>
        [Min(0.0001f)] public float periodTime = 1f;
        
        /// <summary>
        /// 是否立刻执行一次
        /// </summary>
        public bool immediatelyOnce;
        
        /// <summary>
        /// 持续时间计时器
        /// </summary>
        [ProgressBar(min: 0, maxGetter: nameof(duration))]
        public float durationTimer;

        /// <summary>
        /// 周期时间计时器
        /// </summary>
        [ProgressBar(min: 0, maxGetter: nameof(periodTime))]
        public float periodTimer;
        
        /// <summary>
        /// 是否处于暂停状态
        /// </summary>
        public bool isPaused;

        internal void Update(float deltaTime)
        {
            if (immediatelyOnce && durationTimer <= Mathf.Epsilon)
            {
                OnApply();
            }
            
            if (durationTimer >= duration)
            {
                abilitySystem.RemoveEffect(this);
                return;
            }
            
            if (isPaused) return;
            
            durationTimer += deltaTime;
            periodTimer += deltaTime;

            if (periodTimer < periodTime) return;
                
            OnApply();
            periodTimer -= periodTime;
        }
    }
}