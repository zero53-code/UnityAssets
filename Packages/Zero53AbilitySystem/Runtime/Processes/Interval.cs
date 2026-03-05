using System;
using UnityEngine;

namespace Zero53.AbilitySystem.Processes
{
    [Serializable]
    public sealed class Interval<TEffect> : IProcess
        where TEffect : IEffect
    {
        [field: SerializeField, Min(0)] public float intervalTime { get; private set; }

        [SerializeField] private TEffect effect;
        
        private float _timer;

        public Interval(float intervalTime, TEffect effect)
        {
            this.intervalTime = intervalTime;
            this.effect = effect;
        }

        public int count { get; private set; }
        
        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer < intervalTime) return;
            
            effect?.Apply();
            count++;
            _timer -= intervalTime;
        }
    }
}