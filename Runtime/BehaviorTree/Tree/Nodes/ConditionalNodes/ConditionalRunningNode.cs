using System;
using UnityEngine;

namespace Zero53.BehaviorTree.Tree.Nodes.ConditionalNodes
{
    [Serializable]
    public class ConditionalRunningNode : LeafNode
    {
        [SerializeReference] public ICondition condition;

        // public ConditionalRunningNode(Func<bool> condition, string name = "ConditionalRunningNode", int priority = 0) : base(name, priority)
        // {
        //     _condition = condition;
        // }
        
        protected override Status Process()
        {
            try
            {
                return condition.Evaluate() ? Status.Success : Status.Running;
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