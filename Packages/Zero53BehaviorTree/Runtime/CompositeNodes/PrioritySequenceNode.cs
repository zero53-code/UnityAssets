using System.Collections.Generic;
using System.Linq;

namespace Zero53.BehaviorTree.CompositeNodes
{
    public class PrioritySequenceNode : SequenceNode
    {
        private List<Node> _sortedChildren;
        protected override List<Node> sortedChildren => _sortedChildren ??= SortChildren();
        
        public PrioritySequenceNode(string name = "PrioritySequence", int priority = 0, List<Node> children = null) : base(name, priority, children)
        {
        }

        protected virtual List<Node> SortChildren()
        {
            return Children
                .OrderByDescending(node => node.Priority)
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