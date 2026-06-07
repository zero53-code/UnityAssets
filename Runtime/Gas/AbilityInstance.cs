using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.Gas.Abilities;
using Zero53.Gas.Triggers;

namespace Zero53.Gas
{
    [Serializable]
    internal class AbilityInstance
    {
        [OdinSerialize, SerializeField]
        [OnValueChanged("Init")]
        [FoldoutGroup("$ability")]
        public AbilityTrigger trigger;
        
        [OdinSerialize, SerializeReference, InlineEditor]
        [OnValueChanged("Init")]
        [FoldoutGroup("$ability")]
        public GameplayAbility ability;

        [OdinSerialize]
        [OnValueChanged("Init")]
        [FoldoutGroup("$ability")]
        public AbilityTaskDomain taskDomain;

        public AbilityInstance(AbilityTrigger trigger, GameplayAbility ability)
        {
            this.trigger = trigger;
            this.ability = ability;
            taskDomain = new AbilityTaskDomain();
        }

        public void Init(AbilitySystem abilitySystem)
        {
            taskDomain ??= new AbilityTaskDomain();
            
            ability.InitInternal(abilitySystem, trigger, taskDomain);
            taskDomain.Init(ability);
            trigger?.InitInternal(ability);
        }
    }
}