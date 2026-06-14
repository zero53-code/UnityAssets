using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Zero53.Gas.GameplayAbilityTriggers
{
    [Serializable]
    [BoxGroup]
    public sealed class AbilityTrigger : GameplayAbilityTrigger
    {
        [SerializeField]
        private OwnerTagPresentTrigger ownerTagPresent;
        
        [SerializeField]
        private OwnerTagAddedTrigger ownerTagAdded;
        
        [SerializeReference]
        private GameplayAbilityTrigger[] triggers;

        protected internal override void OnInit()
        {
            triggers ??= Array.Empty<GameplayAbilityTrigger>();
            ownerTagPresent ??= new OwnerTagPresentTrigger();
            ownerTagAdded ??= new OwnerTagAddedTrigger();
            
            ownerTagPresent.InitInternal(ability);
            ownerTagAdded.InitInternal(ability);

            foreach (var trigger in triggers)
            {
                trigger?.InitInternal(ability);
            }
        }

        protected internal override void OnUpdate(float deltaTime)
        {
            ownerTagPresent.UpdateInternal(deltaTime);
            ownerTagAdded.UpdateInternal(deltaTime);

            foreach (var trigger in triggers)
            {
                trigger.UpdateInternal(deltaTime);
            }

            var canActivate = ownerTagPresent.isActive;

            if (!canActivate && ability.isActivated)
            {
                ability.Cancel();
                return;
            }
            
            canActivate &= ownerTagAdded.isActive;
            canActivate &= triggers.Length == 0 || triggers.All(trigger => trigger.isActive);
            
            if (canActivate) ActivateAbility();
        }

        protected internal override void OnRemove()
        {
            ownerTagPresent.OnRemove();
            ownerTagAdded.OnRemove();
            foreach (var trigger in triggers)
            {
                trigger?.OnRemove();
            }
        }
    }
//     
// #if UNITY_EDITOR
//
//     internal class AbilityTriggerDrawer : OdinValueDrawer<AbilityTrigger>
//     {
//         protected override void DrawPropertyLayout(GUIContent label)
//         {
//             Property.Children["triggers"].Draw(label);
//         }
//     }
//
// #endif
}