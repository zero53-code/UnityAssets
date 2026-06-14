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
        public GameplayAbilityBase ability { get; private set; }
        public GameplayAbilitySystem owner => ability.owner;

        internal bool isActive { get; set; }

        protected void ActivateAbility()
        {
            isActive = true;

#if UNITY_EDITOR
            GUIHelper.RequestRepaint();
#endif
        }
        
        internal void UpdateInternal(float deltaTime)
        {
#if UNITY_EDITOR
            if (isActive)
            {
                isActive = false;
                GUIHelper.RequestRepaint();
            }
#else
            isActive = false;
#endif
            OnUpdate(deltaTime);
        }

        internal void InitInternal(GameplayAbilityBase ability)
        {
            this.ability = ability;
            OnInit();
        }
        
        protected internal virtual void OnInit() 
        {}
        
        protected internal abstract void OnUpdate(float deltaTime);
        
        protected internal virtual void OnRemove() 
        {}
    }

#if UNITY_EDITOR

    internal class AbilityTriggerBaseDrawer : OdinValueDrawer<GameplayAbilityTrigger>
    {
        private const string ActivatingIconGuid = "4650f35f8a2329341b730712e2c273a2";
        private const string NonActivatingIconGuid = "06af4300f90db43478c98ffcd0282b39";
        
        private static Texture _activatingIcon;
        private static Texture _nonActivatingIcon;
        
        private const float DelayTime = 0.8f;
        
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

        private float _activatingLastTime;
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var isActive = ValueEntry.SmartValue.isActive;
            if (isActive)
            {
                _activatingLastTime = Time.time;
            }
            
            label ??= new GUIContent(Property.ValueEntry.TypeOfValue.Name);

            switch (isActive)
            {
                case false when Application.isPlaying && Time.time - _activatingLastTime > DelayTime:
                case false:
                    label.image = _nonActivatingIcon;
                    break;
                default:
                    label.image = _activatingIcon;
                    break;
            }

            CallNextDrawer(label);
        }
    }

#endif
}