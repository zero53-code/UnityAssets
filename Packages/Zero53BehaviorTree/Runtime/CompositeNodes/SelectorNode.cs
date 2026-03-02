using System.Collections.Generic;

namespace Zero53.BehaviorTree.CompositeNodes
{
    /// <summary>
    /// 选择节点
    /// </summary>
    public class SelectorNode : CompositeNode
    {
        public SelectorNode(string name = "Select", int priority = 0, List<Node> children = null) : base(name, priority, children)
        {
        }

        protected override Status Process()
        {
            if (CurrentChild >= Children.Count)
            {
                Reset();
                return Status.Failure;
            }

            switch (Children[CurrentChild].ExecuteProcess())
            {
                case Status.Success:
                    return Status.Success;
                case Status.Running:
                    return Status.Running;
                case Status.Failure:
                default:
                    CurrentChild++;
                    return Status.Running;
            }
        }
    }
}