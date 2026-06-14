using System;
using System.ComponentModel;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using Zero53.GameplayTags;

namespace Zero53.Gas.GameplayAbilityTriggers
{
    [Serializable]
    [Description("拥有该 Tag 期间持续触发")]
    public class OwnerTagPresentTrigger : GameplayAbilityTrigger
    {
        /// <summary>
        /// 激活技能的标签
        /// </summary>
        [SerializeField] private Tag[] tags;

        protected internal override void OnUpdate(float deltaTime)
        {
            if (tags.Length == 0 || owner.tags.HasAny(tags))
            {
                ActivateAbility();
            }
        }
    }

#if UNITY_EDITOR

    internal class OwnerTagPresentTriggerDrawer : OdinValueDrawer<OwnerTagPresentTrigger>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Property.Children["tags"].Draw(label);
        }
    }

#endif

}