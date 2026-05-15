using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero53.Gas.Plugins.Zero53.Runtime.Gas.Effects
{
    /// <summary>
    /// 周期效果
    /// </summary>
    [Serializable]
    public class PeriodEffect : IGameplayEffect
    {
        [SerializeReference] public IGameplayEffect effect;
        
        [Min(0f)] public float duration = float.PositiveInfinity;
        
        [Min(0f)] public float period;
        
        public bool immediatelyOnce;
        
        [ProgressBar(min: 0, maxGetter: "duration")]
        public float durationTimer;

        [ProgressBar(min: 0, maxGetter: "period")]
        public float periodTimer;
        
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