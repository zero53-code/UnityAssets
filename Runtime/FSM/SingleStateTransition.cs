using System;

namespace Zero53.Fsm
{
    public delegate bool TransitionCondition();
    
    public class SingleStateTransition : IStateTransition
    {
        private readonly IState _from;
        private readonly TransitionCondition _condition;

        public SingleStateTransition(IState from, IState to, TransitionCondition condition)
        {
            this._from = from ?? throw new ArgumentNullException(nameof(from));
            this.to = to ?? throw new ArgumentNullException(nameof(to));
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public bool FromCondition(IState state)
        {
            return ReferenceEquals(_from, state);
        }

        public IState to { get; }
        
        public bool Condition()
        {
            return _condition();
        }
    }
}