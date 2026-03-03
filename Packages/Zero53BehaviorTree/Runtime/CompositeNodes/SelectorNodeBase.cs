using System.Collections.Generic;

namespace Zero53.BehaviorTree.CompositeNodes
{
    public struct SelectorNodeBase
    {
        public CompositeNodeBase compositeNode { get; private set; }
        private int _currentChildIndex;
        
        public SelectorNodeBase(string name = "Node", int priority = 0, List<INode> children = null)
        {
            compositeNode = new CompositeNodeBase(name, priority, children);
            _currentChildIndex = 0;
        }

        public NodeStatus Process(IList<INode> children)
        {
            if (_currentChildIndex >= compositeNode.childCount)
            {
                compositeNode.Reset();
                return NodeStatus.Failure;
            }

            switch (children[_currentChildIndex].Process())
            {
                case NodeStatus.Success:
                    return NodeStatus.Success;
                case NodeStatus.Running:
                    return NodeStatus.Running;
                case NodeStatus.Failure:
                default:
                    _currentChildIndex++;
                    return NodeStatus.Running;
            }
        }
    }
}