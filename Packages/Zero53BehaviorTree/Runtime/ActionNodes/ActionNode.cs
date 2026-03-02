using System;

namespace Zero53.BehaviorTree.ActionNodes
{
    public class ActionNode : LeafNode
    {
        private readonly Action _doSomething;
        public ActionNode(Action doSomething, string name = "ActionNode", int priority = 0) : base(name, priority)
        {
            _doSomething = doSomething;
        }

        protected override Status Process()
        {
            try
            {
                _doSomething();
                return Status.Success;
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