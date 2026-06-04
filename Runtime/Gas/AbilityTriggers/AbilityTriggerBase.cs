using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Zero53.Gas.Abilities;

namespace Zero53.Gas.AbilityTriggers
{
    [Serializable]
    public abstract class AbilityTriggerBase
    {
        public GameplayAbility ability { get; internal set; }
        public AbilitySystem abilitySystem => ability.abilitySystem;

        internal bool isActive { get; set; }

        protected void ActivateAbility()
        {
            isActive = true;
        }
        
        internal void Update(float deltaTime)
        {
            isActive = false;
            OnUpdate(deltaTime);
        }
        
        protected internal virtual void OnInit() {}
        protected internal abstract void OnUpdate(float deltaTime);
    }

#if UNITY_EDITOR

    internal class AbilityTriggerBaseDrawer : OdinValueDrawer<AbilityTriggerBase>
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