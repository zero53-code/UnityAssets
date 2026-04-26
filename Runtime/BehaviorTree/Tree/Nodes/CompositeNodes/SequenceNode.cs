using System;
using System.Collections.Generic;

namespace Zero53.BehaviorTree.Tree.Nodes.CompositeNodes
{
    /// <summary>
    /// 序列节点
    /// </summary>
    [Serializable]
    public class SequenceNode : CompositeNode
    {
        protected virtual List<Node> sortedChildren => children;
        
        // public SequenceNode(string name = "Sequence", int priority = 0, List<Node> children = null) : base(name, priority, children)
        // {
        // }

        protected override Status Process()
        {
            while (currentChild < sortedChildren.Count)
            {
                switch (sortedChildren[currentChild].ExecuteProcess())
                {
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                    default:
                        currentChild++;
                        return currentChild == children.Count ? Status.Success : Status.Running;
                }
            }
            Reset();
            return Status.Success;
        }
    }
}