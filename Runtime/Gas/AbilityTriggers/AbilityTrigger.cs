using System;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Zero53.Gas.AbilityTriggers
{
    [Serializable]
    public sealed class AbilityTrigger : AbilityTriggerBase
    {
        [SerializeReference]
        private AbilityTriggerBase[] triggers;

        protected internal override void Init()
        {
            triggers ??= Array.Empty<AbilityTriggerBase>();

            foreach (var trigger in triggers)
            {
                trigger.InitInternal(ability);
            }
        }

        protected internal override void Update(float deltaTime)
        {
            if (triggers == null || triggers.Length == 0) return;
            
            
            foreach (var trigger in triggers)
            {
                trigger.UpdateInternal(deltaTime);
            }

            var canActivate = true;
            foreach (var trigger in triggers)
            {
                if (trigger.isActive) continue;
                canActivate = false;
                break;
            }

            if (canActivate) ActivateAbility();
        }
    }
    
#if UNITY_EDITOR

    internal class AbilityTriggerDrawer : OdinValueDrawer<AbilityTrigger>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Property.Children["triggers"].Draw(label);
        }
    }

#endif
}