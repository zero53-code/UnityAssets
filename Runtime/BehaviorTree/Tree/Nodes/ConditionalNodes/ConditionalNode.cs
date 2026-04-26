using System;
using UnityEngine;

namespace Zero53.BehaviorTree.Tree.Nodes.ConditionalNodes
{
    [Serializable]
    public class ConditionalNode : LeafNode
    {
        [SerializeReference]
        public ICondition condition;
        
        // public ConditionalNode(Func<bool> condition, string name = "ConditionalNode", int priority = 0) : base(name, priority)
        // {
        //     _condition = condition;
        // }
        
        protected override Status Process()
        {
            try
            {
                return condition.Evaluate() ? Status.Success : Status.Failure;
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