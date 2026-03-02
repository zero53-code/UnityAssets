using System.Collections.Generic;

namespace Zero53.AbilitySystem.Buffs
{
    public class BuffSystem : IProcess
    {
        private readonly HashSet<IBuff> _buffs = new();
        private readonly HashSet<IBuff> _addPendingBuffs = new();
        private readonly HashSet<IBuff> _removePendingBuffs = new();

        public void OnUpdate(float deltaTime)
        {
            _buffs.UnionWith(_addPendingBuffs);
            _addPendingBuffs.Clear();
            
            foreach (var buff in _buffs)
            {
                if (buff.isEnd)
                {
                    _removePendingBuffs.Add(buff);
                    buff.OnEnd();
                }
            }
            _buffs.ExceptWith(_removePendingBuffs);
            _removePendingBuffs.Clear();

            foreach (var buff in _buffs)
            {
                buff.OnUpdate(deltaTime);
            }
        }

        public void AddBuff(IBuff buff)
        {
            _removePendingBuffs.Remove(buff);
            _addPendingBuffs.Add(buff);
            buff.OnApply();
        }

        public void RemoveBuff(IBuff buff)
        {
            _addPendingBuffs.Remove(buff);
            _removePendingBuffs.Add(buff);
            buff.OnEnd();
        }
    }
}