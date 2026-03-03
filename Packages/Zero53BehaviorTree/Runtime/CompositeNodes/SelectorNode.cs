using System;
using System.Collections.Generic;

namespace Zero53.BehaviorTree.CompositeNodes
{
    /// <summary>
    /// 选择节点
    /// </summary>
    public class SelectorNode : ICompositeNode
    {
        private SelectorNodeBase _base;
        public SelectorNode(string name = "Select", int priority = 0, List<INode> children = null)
        {
            _base = new SelectorNodeBase(name, priority, children);
        }

        public int priority => _base.compositeNode.priority;

        public NodeStatus Process()
        {
            return _base.Process(_base.compositeNode.children);
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