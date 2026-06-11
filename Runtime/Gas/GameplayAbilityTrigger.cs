using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Zero53.Gas
{
    [Serializable]
    public abstract class GameplayAbilityTrigger
    {
        public GameplayAbility ability { get; private set; }
        public GameplayAbilitySystem abilitySystem => ability.abilitySystem;

        internal bool isActive { get; set; }

        protected void ActivateAbility()
        {
            isActive = true;
        }
        
        internal void UpdateInternal(float deltaTime)
        {
            isActive = false;
            OnUpdate(deltaTime);
        }

        internal void InitInternal(GameplayAbility ability)
        {
            this.ability = ability;
            OnInit();
        }
        
        protected internal virtual void OnInit() {}
        protected internal abstract void OnUpdate(float deltaTime);
    }

#if UNITY_EDITOR

    internal class AbilityTriggerBaseDrawer : OdinValueDrawer<GameplayAbilityTrigger>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var style = GUIStyle.none;
            if (ValueEntry.SmartValue.isActive)
            {
                style.normal.textColor = Color.green;
                SirenixEditorGUI.MessageBox("Activating", MessageType.None, style, true);
            }
            else
            {
                style.normal.textColor = Color.red;
                SirenixEditorGUI.MessageBox("Non Activated", MessageType.None, style, true);
            }
            
            CallNextDrawer(label);
        }
    }

#endif
}