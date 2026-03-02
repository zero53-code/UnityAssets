using System.Collections.Generic;

namespace Zero53.AbilitySystem.Buffs
{
    public class CompoundBuff : IBuff
    {
        private readonly List<IBuff> _buffs;

        public CompoundBuff(List<IBuff> buffs)
        {
            _buffs = buffs;
        }

        public void OnUpdate(float deltaTime)
        {
            for (var i = 0; i < _buffs.Count; i++)
            {
                _buffs[i].OnUpdate(deltaTime);
            }
        }

        public bool isEnd
        {
            get
            {
                var allEnded = true;

                for (var i = 0; i < _buffs.Count; i++)
                {
                    allEnded = allEnded && _buffs[i].isEnd;
                }
                
                return allEnded;
            }
        }

        public void OnEnd()
        {
            for (var i = 0; i < _buffs.Count; i++)
            {
                _buffs[i].OnEnd();
            }
        }

        public void OnApply()
        {
            for (var i = 0; i < _buffs.Count; i++)
            {
                _buffs[i].OnApply();
            }
        }
    }
}