namespace Zero53.Fsm
{
    public interface IStateTransition
    {
        bool FromCondition(IState state);
        
        IState to { get; }
        bool Condition();
    }
}