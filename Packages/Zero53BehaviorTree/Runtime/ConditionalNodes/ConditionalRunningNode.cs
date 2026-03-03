using System;

namespace Zero53.BehaviorTree.ConditionalNodes
{
    public class ConditionalRunningNode : INode
    {
        private LeafNodeBase _base;
        private readonly Func<bool> _condition;
        
        public ConditionalRunningNode(string name = "ConditionalRunningNode", int priority = 0, Func<bool> condition = null)
        {
            _base = new LeafNodeBase(name, priority);
            _condition = condition;
        }

        public int priority => _base.priority;

        public NodeStatus Process()
        {
            return _condition() ? NodeStatus.Success : NodeStatus.Running;
        }

        public void Reset()
        {
        }
    }
}