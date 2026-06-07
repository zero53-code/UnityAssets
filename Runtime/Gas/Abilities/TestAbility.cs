using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.AbilityTasks;
using Zero53.Gas.AbilityTriggers;

namespace Zero53.Gas.Abilities
{
    [CreateAssetMenu(menuName = "Zero53/Gas/Test Ability", fileName = "New Test Ability")]
    public class TestAbility : GameplayAbility
    {
        public float duration = 5f;
        
        protected internal override AbilityTask Commit()
        {
            Debug.Log("TestAbility executed");
            
            return new TestTask(duration);
        }
        
        [Serializable]
        public class TestTask : AbilityTask
        {
            public float duration;
            [ProgressBar(min: 0, maxGetter: "duration")]
            public float timer;
            
            public TestTask(float duration)
            {
                this.duration = duration;
            }
            
            protected internal override void Update(float deltaTime)
            {
                timer += deltaTime;
                if (timer < duration) return;
                
                End();
                Debug.Log("TestTask Ended");
            }
        }
        
        [Serializable]
        public class TestTrigger : AbilityTriggerBase
        {
            public float interval = 5f;

            private float _timer;
            
            protected internal override void Update(float deltaTime)
            {
                if (isActive)
                {
                    _timer = 0;
                    isActive = false;
                }
                    
                _timer += deltaTime;
                isActive = _timer >= interval;
            }
        }
    }
}