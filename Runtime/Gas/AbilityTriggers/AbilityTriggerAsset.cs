using Sirenix.Serialization;
using UnityEngine;

namespace Zero53.Gas.AbilityTriggers
{
    [CreateAssetMenu(menuName = "Zero53/Gas/Create AbilityTrigger", fileName = "New AbilityTrigger")]
    public class AbilityTriggerAsset : ScriptableObject
    {
        [field: OdinSerialize, SerializeReference] public IAbilityTrigger trigger { get; private set; }
    }
}