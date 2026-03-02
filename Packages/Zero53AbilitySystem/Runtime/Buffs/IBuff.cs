namespace Zero53.AbilitySystem.Buffs
{
    public interface IBuff : IProcess, IProcessEnd, IProcessEndEvent
    {
        /// <summary>
        /// Buff 施加到对象时调用
        /// </summary>
        void OnApply();
    }
}