using System.Collections.Generic;

namespace Zero53.BehaviorTree.CompositeNodes
{
    public struct SequenceNodeBase
    {
        public CompositeNodeBase compositeNode { get; private set; }
        private int _currentChildIndex;
        
        public SequenceNodeBase(string name = "Node", int priority = 0, List<INode> children = null)
        {
            compositeNode = new CompositeNodeBase(name, priority, children);
            _currentChildIndex = 0;
        }

        public NodeStatus Process(IList<INode> children)
        {
            if (_currentChildIndex >= children.Count)
            {
                compositeNode.Reset();
                _currentChildIndex = 0;
                return NodeStatus.Success;
            }

            switch (children[_currentChildIndex].Process())
            {
                case NodeStatus.Failure:
                    compositeNode.Reset();
                    return NodeStatus.Failure;
                case NodeStatus.Running:
                    return NodeStatus.Running;
                case NodeStatus.Success:
                default:
                    _currentChildIndex++;
                    return _currentChildIndex == compositeNode.childCount 
                        ? NodeStatus.Success 
                        : NodeStatus.Running;
            }
        }
    }
}