using System;
using UnityEngine;
using Zero53.AbilitySystem.Processes;

namespace Zero53.AbilitySystem.Buffs
{
    [Serializable]
    public class DurationBuff<TBuff> : IBuff
        where TBuff : IBuff
    {
        [SerializeField] private TBuff buff;

        private Timer _timer;

        public DurationBuff(TBuff buff, float duration)
        {
            this.buff = buff;
            _timer = new Timer(duration);
        }

        public void OnUpdate(float deltaTime)
        {
            _timer.OnUpdate(deltaTime);
            if (_timer.isEnd) return;
            
            buff.OnUpdate(deltaTime);
        }

        public bool isEnd => _timer.isEnd || buff.isEnd;
        
        public void OnEnd()
        {
            buff.OnEnd();
        }

        public void OnApply()
        {
            buff.OnApply();
        }
    }
}