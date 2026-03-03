using System;
using System.Collections.Generic;
using System.Linq;
using Zero53.Utils;

namespace Zero53.BehaviorTree.CompositeNodes
{
    public class RandomSelectorNode : ICompositeNode
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
        
        public RandomSelectorNode(string name = "RandomSelector", int priority = 0, List<INode> children = null)
        {
            _base = new SequenceNodeBase(name, priority, children);
            _sortedChildren = children?.ToList() ?? new List<INode>();
            _sorted = false;
        }
        
        private void SortChildren()
        {
            _sorted = true;
            _sortedChildren.Shuffle();
        }

        public int priority => _base.compositeNode.priority;

        public NodeStatus Process()
        {
            return _base.Process(sortedChildren);
        }

        public void Reset()
        {
            _base.compositeNode.Reset();
        }

        public IList<INode> children => _base.compositeNode.children;

        public void AddChild(INode child)
        {
            _base.compositeNode.AddChild(child);
        }
    }
}