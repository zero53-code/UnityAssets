using System;

namespace Zero53.BehaviorTree.DecoratorNodes
{
    public class ExceptionHandlingNode : ISingleChildNode
    {
        private DecoratorNodeBase _base;
        private ExceptionHandling _exceptionHandling;

        public ExceptionHandlingNode(string name = "ExceptionHandling", int priority = 0, INode child = null,
            ExceptionHandling exceptionHandling = ExceptionHandling.Throw)
        {
            _base = new DecoratorNodeBase(name, priority, child);
            this.child = child;
            _exceptionHandling = exceptionHandling;
        }

        public int priority => _base.nodeBase.priority;
        public NodeStatus Process()
        {
            try
            {
                return child.Process();
            }
            catch
            {
                switch (_exceptionHandling)
                {
                    case ExceptionHandling.ReturnSuccess:
                        return NodeStatus.Success;
                    case ExceptionHandling.ReturnFailure:
                        return NodeStatus.Failure;
                    case ExceptionHandling.Throw:
                    default:
                        throw;
                }
            }
        }

        public void Reset()
        {
        }

        public INode child { get; set; }
    }
}