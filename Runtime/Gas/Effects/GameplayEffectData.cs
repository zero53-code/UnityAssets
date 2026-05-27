using Sirenix.Serialization;
using UnityEngine;
using Zero53.Gas.AttributeSet;

namespace Zero53.Gas.Effects
{
    [CreateAssetMenu(menuName = "Zero53/Create GameplayEffect", fileName = "New GameplayEffect")]
    public class GameplayEffectData : ScriptableObject, IGameplayEffect
    {
        [field: OdinSerialize, SerializeReference]
        public IGameplayEffect[] effects { get; private set; }

        public void Apply(GameplayAttributeSet target)
        {
            if (effects == null) return;
            
            foreach (var effect in effects)
            {
                effect.Apply(target);
            }
        }
    }
}