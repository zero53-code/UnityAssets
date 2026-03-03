using System;
using System.Collections.Generic;
using System.Linq;

namespace Zero53.BehaviorTree.CompositeNodes
{
    public class PrioritySelectorNode : ICompositeNode
    {
        private SequenceNodeBase _base;
        private readonly List<INode> _sortedChildren;
        private bool _sorted;

        private List<INode> sortedChildren
        {
            get
            {
                if (_sorted) return _sortedChildren;
                SortChildren();
                return _sortedChildren;
            }
        }
        
        public PrioritySelectorNode(string name = "PrioritySelector", int priority = 0, List<INode> children = null)
        {
            _base = new SequenceNodeBase(name, priority, children);
            _sortedChildren = children?.ToList() ?? new List<INode>();
            _sorted = false;
        }

        private void SortChildren()
        {
            _sortedChildren.Sort((node1, node2) => node1.priority.CompareTo(node2.priority));
            _sorted = true;
        }
        
        public int priority => _base.compositeNode.priority;

        public NodeStatus Process()
        {
            return _base.Process(sortedChildren);
        }

        public void Reset()
        {
            _base.compositeNode.Reset();
            _sorted = false;
        }

        public IList<INode> children => _base.compositeNode.children;

        public void AddChild(INode child)
        {
            _base.compositeNode.AddChild(child);
            _sorted = false;
        }
    }
}