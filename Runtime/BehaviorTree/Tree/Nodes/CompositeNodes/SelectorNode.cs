using System;
using System.Collections.Generic;

namespace Zero53.BehaviorTree.Tree.Nodes.CompositeNodes
{
    /// <summary>
    /// 选择节点
    /// </summary>
    [Serializable]
    public class SelectorNode : CompositeNode
    {
        // public SelectorNode(string name = "Select", int priority = 0, List<Node> children = null) : base(name, priority, children)
        // {
        // }

        protected override Status Process()
        {
            if (currentChild >= children.Count)
            {
                Reset();
                return Status.Failure;
            }

            switch (children[currentChild].ExecuteProcess())
            {
                case Status.Success:
                    return Status.Success;
                case Status.Running:
                    return Status.Running;
                case Status.Failure:
                default:
                    currentChild++;
                    return Status.Running;
            }
        }
    }
}