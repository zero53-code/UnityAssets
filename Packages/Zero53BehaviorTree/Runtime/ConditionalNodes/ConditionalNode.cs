using System;

namespace Zero53.BehaviorTree.ConditionalNodes
{
    public class ConditionalNode : LeafNode
    {
        private readonly Func<bool> _condition;
        
        public ConditionalNode(Func<bool> condition, string name = "ConditionalNode", int priority = 0) : base(name, priority)
        {
            _condition = condition;
        }
        
        protected override Status Process()
        {
            try
            {
                return _condition() ? Status.Success : Status.Failure;
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