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

        [SerializeField] private Timer timer;

        public DurationBuff(TBuff buff, float duration)
        {
            this.buff = buff;
            timer = new Timer(duration);
        }

        public void OnUpdate(float deltaTime)
        {
            timer.OnUpdate(deltaTime);
            if (timer.isEnd) return;
            
            buff.OnUpdate(deltaTime);
        }

        public bool isEnd => timer.isEnd || buff.isEnd;
        
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