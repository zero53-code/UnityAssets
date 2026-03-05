using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zero53.AbilitySystem.Buffs
{
    [Serializable]
    public class CompoundBuff : IBuff
    {
        [SerializeReference] private List<IBuff> buffs;

        public CompoundBuff(List<IBuff> buffs)
        {
            this.buffs = buffs;
        }

        public void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < buffs.Count; i++)
            {
                buffs[i].OnUpdate(deltaTime);
            }
        }

        public bool isEnd
        {
            get
            {
                var allEnded = true;

                for (var i = 0; i < buffs.Count; i++)
                {
                    allEnded = allEnded && buffs[i].isEnd;
                }
                
                return allEnded;
            }
        }

        public void OnEnd()
        {
            for (var i = 0; i < buffs.Count; i++)
            {
                buffs[i].OnEnd();
            }
        }

        public void OnApply()
        {
            for (var i = 0; i < buffs.Count; i++)
            {
                buffs[i].OnApply();
            }
        }
    }
}