using System;

namespace Zero53.Fsm
{
    public delegate bool TransitionFromCondition(IState state);
    public class FromConditionStateTransition : IStateTransition
    {
        private readonly TransitionFromCondition _fromCondition;
        private readonly TransitionCondition _condition;
        public FromConditionStateTransition(TransitionFromCondition fromCondition, IState to, TransitionCondition condition)
        {
            this._fromCondition = fromCondition ?? throw new ArgumentNullException(nameof(fromCondition));
            this.to = to ?? throw new ArgumentNullException(nameof(to));
            this._condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }
        
        public bool FromCondition(IState state)
        {
            return _fromCondition(state);
        }

        public IState to { get; }
        public bool Condition()
        {
            return _condition();
        }
    }
}