using System.Collections.Generic;

namespace Zero53.BehaviorTree.CompositeNodes
{
    /// <summary>
    /// 序列节点
    /// </summary>
    public class SequenceNode : CompositeNode
    {
        protected virtual List<Node> sortedChildren => Children;
        
        public SequenceNode(string name = "Sequence", int priority = 0, List<Node> children = null) : base(name, priority, children)
        {
        }

        protected override Status Process()
        {
            while (CurrentChild < sortedChildren.Count)
            {
                switch (sortedChildren[CurrentChild].ExecuteProcess())
                {
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                    default:
                        CurrentChild++;
                        return CurrentChild == Children.Count ? Status.Success : Status.Running;
                }
            }
            Reset();
            return Status.Success;
        }
    }
}