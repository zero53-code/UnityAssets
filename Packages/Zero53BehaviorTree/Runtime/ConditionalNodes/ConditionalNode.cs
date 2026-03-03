using System;

namespace Zero53.BehaviorTree.ConditionalNodes
{
    public class ConditionalNode : INode
    {
        private LeafNodeBase _base;
        private readonly Func<bool> _condition;
        
        public ConditionalNode(Func<bool> condition, string name = "ConditionalNode", int priority = 0)
        {
            _base = new LeafNodeBase(name, priority);
            _condition = condition;
        }

        public int priority => _base.priority;

        public NodeStatus Process()
        {
            return _condition() ? NodeStatus.Success : NodeStatus.Failure;
        }

        public void Reset()
        {
        }
    }
}