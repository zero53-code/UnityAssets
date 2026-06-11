using System;
using Sirenix.OdinInspector.Editor;
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
        private const string ActivatingIconGuid = "4650f35f8a2329341b730712e2c273a2";
        private const string NonActivatingIconGuid = "06af4300f90db43478c98ffcd0282b39";
        
        private static Texture _activatingIcon;
        private static Texture _nonActivatingIcon;
        
        protected override void Initialize()
        {
            LoadActivatingIconGuid();
            LoadNonActivatingIconGuid();
        }

        private static void LoadActivatingIconGuid()
        {
            if (_activatingIcon != null) return;

            var path = AssetDatabase.GUIDToAssetPath(ActivatingIconGuid);

            if (!string.IsNullOrEmpty(path))
            {
                _activatingIcon = AssetDatabase.LoadAssetAtPath<Texture>(path);
            }
        }
        
        private static void LoadNonActivatingIconGuid()
        {
            if (_nonActivatingIcon != null) return;

            var path = AssetDatabase.GUIDToAssetPath(NonActivatingIconGuid);

            if (!string.IsNullOrEmpty(path))
            {
                _nonActivatingIcon = AssetDatabase.LoadAssetAtPath<Texture>(path);
            }
        }
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (label == null)
            {
                CallNextDrawer(null);
                return;
            }
            
            label.image = ValueEntry.SmartValue.isActive 
                ? _activatingIcon 
                : _nonActivatingIcon;

            CallNextDrawer(label);
        }
    }

#endif
}