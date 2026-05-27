using System;
using UnityEngine;
using Zero53.Gas.AttributeSet;
using Random = UnityEngine.Random;

namespace Zero53.Gas.Effects
{
    /// <summary>
    /// 随机效果
    /// </summary>
    [Serializable]
    public class RandomEffect : IGameplayEffect
    {
        [SerializeReference] public IGameplayEffect[] effects;

        public void Apply(GameplayAttributeSet target)
        {
            if (effects == null || effects.Length == 0) return;

            var randomIndex = Random.Range(0, effects.Length);
            effects[randomIndex].Apply(target);
        }
    }
}