using System;

namespace Zero53.BehaviorTree.ConditionalNodes
{
    public class ConditionalRunningNode : LeafNode
    {
        private readonly Func<bool> _condition;
        
        public ConditionalRunningNode(Func<bool> condition, string name = "ConditionalRunningNode", int priority = 0) : base(name, priority)
        {
            _condition = condition;
        }
        
        protected override Status Process()
        {
            try
            {
                return _condition() ? Status.Success : Status.Running;
            }
            catch
            {
                switch (exceptionHandling)
                {
                    case ExceptionHandling.ReturnSuccess:
                        return Status.Success;
                    case ExceptionHandling.ReturnFailure:
                        return Status.Failure;
                    case ExceptionHandling.Throw:
                    default:
                        throw;
                }
            }
        }
    }
}