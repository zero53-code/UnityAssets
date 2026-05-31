using System;
using UnityEngine;
using Zero53.Gas.AttributeSets;

namespace Zero53.Gas.Effects
{
    /// <summary>
    /// 组合效果
    /// </summary>
    [Serializable]
    public class CombinationEffect : IGameplayEffect
    {
        [SerializeReference] public IGameplayEffect[] effects;

        public void Apply(AbilitySystem target, float deltaTime)
        {
            if (effects == null) return;
            
            foreach (var effect in effects)
            {
                effect.Apply(target, deltaTime);
            }
        }
    }
}