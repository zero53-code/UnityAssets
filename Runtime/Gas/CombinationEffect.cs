using System;
using UnityEngine;

namespace Zero53.Gas
{
    /// <summary>
    /// 组合效果
    /// </summary>
    [Serializable]
    public class CombinationEffect : GameplayEffect
    {
        [SerializeReference] public GameplayEffect[] effects;

        protected internal override void Update(float deltaTime)
        {
            if (effects == null) return;
            
            foreach (var effect in effects)
            {
                effect.Update(deltaTime);
            }
        }

        protected internal override void Apply()
        {
            if (effects == null) return;
            
            foreach (var effect in effects)
            {
                effect.Apply();
            }
        }
    }
}