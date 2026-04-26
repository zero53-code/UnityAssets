using System;
using System.Collections.Generic;
using System.Linq;

namespace Zero53.BehaviorTree.Tree.Nodes.CompositeNodes
{
    [Serializable]
    public class PrioritySequenceNode : SequenceNode
    {
        private List<Node> _sortedChildren;
        protected override List<Node> sortedChildren => _sortedChildren ??= SortChildren();
        
        // public PrioritySequenceNode(string name = "PrioritySequence", int priority = 0, List<Node> children = null) : base(name, priority, children)
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
    }
}