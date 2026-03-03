using System;
using System.Collections.Generic;

namespace Zero53.BehaviorTree.CompositeNodes
{
    /// <summary>
    /// 序列节点
    /// </summary>
    public class SequenceNode : ICompositeNode
    {
        private SequenceNodeBase _base;
        
        public SequenceNode(string name = "Sequence", int priority = 0, List<INode> children = null)
        {
            _base = new SequenceNodeBase(name, priority, children);
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