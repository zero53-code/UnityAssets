using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Zero53.Gas.Abilities;
using Zero53.Gas.AbilityTasks;
using Zero53.Gas.AbilityTriggers;

namespace Zero53.Gas
{
    [Serializable]
    internal struct AbilityInfo
    {
        [OdinSerialize, SerializeField]
        [OnValueChanged("Init")]
        public AbilityTrigger trigger;
        
        [OdinSerialize, SerializeReference, InlineEditor]
        [OnValueChanged("Init")]
        public GameplayAbility ability;

        [OdinSerialize]
        [OnValueChanged("Init")]
        public AbilityTaskDomain taskDomain;

        public AbilityInfo(AbilityTrigger trigger, GameplayAbility ability)
        {
            this.trigger = trigger;
            this.ability = ability;
            taskDomain = new AbilityTaskDomain();
            Init();
        }

        public void Init()
        {
            taskDomain ??= new AbilityTaskDomain();
            ability.domain = taskDomain;
            taskDomain.ability = ability;
            
            if (trigger == null) return;
            
            trigger.ability = ability;
            ability.trigger = trigger;
            
            trigger.OnInit();
        }
    }
}