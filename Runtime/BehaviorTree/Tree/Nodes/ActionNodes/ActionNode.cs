using System;
using UnityEngine;

namespace Zero53.BehaviorTree.Tree.Nodes.ActionNodes
{
    [Serializable]
    public class ActionNode : LeafNode
    {
        [SerializeReference] public IAction doSomething;
        // public ActionNode(Action doSomething, string name = "ActionNode", int priority = 0) : base(name, priority)
        // {
        //     _doSomething = doSomething;
        // }

        protected override Status Process()
        {
            try
            {
                doSomething?.Invoke();
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