using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.Gas.GameplayTriggers;

namespace Zero53.Gas
{
    [Serializable]
    internal class GameplayAbilityInstance
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
        public GameplayAbilityTaskDomain taskDomain;

        public GameplayAbilityInstance(AbilityTrigger trigger, GameplayAbility ability)
        {
            this.trigger = trigger;
            this.ability = ability;
            taskDomain = new GameplayAbilityTaskDomain();
        }

        public void Init(GameplayAbilitySystem abilitySystem)
        {
            taskDomain ??= new GameplayAbilityTaskDomain();
            
            ability.InitInternal(abilitySystem, trigger, taskDomain);
            taskDomain.Init(ability);
            trigger?.InitInternal(ability);
        }
    }
}