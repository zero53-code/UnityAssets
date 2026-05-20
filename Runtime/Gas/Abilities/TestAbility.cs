using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.Gas.AbilityTriggers;

namespace Zero53.Gas.Abilities
{
    [Serializable]
    public class TestAbility : GameplayAbility
    {
        public float duration = 5f;

        [Serializable]
        public class TestTrigger : IAbilityTrigger
        {
            public float interval = 5f;

            private float _timer;
            public bool Check(float deltaTime)
            {
                _timer += deltaTime;
                
                if (_timer < interval) return false;
                _timer -= interval;
                return true;
            }
        }
        
        [Serializable]
        public class TestTask : AbilityTask
        {
            public float duration;
            [ProgressBar(min: 0, maxGetter: "duration")]
            public float timer;
            
            public TestTask(float duration, GameplayAbility ability) : base(ability)
            {
                this.duration = duration;
            }
            
            protected internal override void OnUpdate(float deltaTime)
            {
                timer += deltaTime;
                if (timer < duration) return;
                
                End();
                Debug.Log("TestTask Ended");
            }
        }
        
        protected internal override void Execute()
        {
            Debug.Log("TestAbility executed");
            
            domain.AddAbilityTask(new TestTask(duration, this));
        }
    }
}