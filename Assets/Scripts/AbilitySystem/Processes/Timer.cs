using System;
using UnityEngine;

namespace Zero53.AbilitySystem.Processes
{
    [Serializable]
    public sealed class Timer : IProcess, IProcessEnd
    {
        [field: SerializeField, Min(0)] public float duration { get; private set; }

        [field: SerializeField] private float elapsedTime;

        public Timer(float duration)
        {
            this.duration = duration;
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (elapsedTime < duration)
            {
                elapsedTime += deltaTime;
            }
            else
            {
                isEnd = true;
            }
        }
        
        public bool isEnd { get; private set; }

        public float elapsed => elapsedTime;
    }
}