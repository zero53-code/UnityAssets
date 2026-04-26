using System;
using System.Collections.Generic;
using System.Linq;

namespace Zero53.BehaviorTree.Tree.Nodes.CompositeNodes
{
    [Serializable]
    public class PrioritySelectorNode : SelectorNode
    {
        private List<Node> _sortedChildren;
        private List<Node> sortedChildren => _sortedChildren ??= SortChildren();
        
        // public PrioritySelectorNode(string name = "PrioritySelector", int priority = 0, List<Node> children = null) : base(name, priority, children)
        // {
        // }

        protected virtual List<Node> SortChildren()
        {
            return children
                .OrderByDescending(node => node.priority)
                .ToList();
        }

        public override void Reset()
        {
            base.Reset();
            _sortedChildren = null;
        }

        public override void AddChild(Node child)
        {
            base.AddChild(child);
            _sortedChildren = null;
        }

        protected override Status Process()
        {
            if (currentChild >= sortedChildren.Count)
            {
                Reset();
                return Status.Failure;
            }

            switch (sortedChildren[currentChild].ExecuteProcess())
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