namespace Zero53.BehaviorTree.DecoratorNodes
{
    public struct DecoratorNodeBase
    {
        public NodeBase nodeBase { get; private set; }
        private INode _childNode;

        public DecoratorNodeBase(string name = "Node", int priority = 0, INode child = null)
        {
            nodeBase = new NodeBase(name, priority);
            _childNode = child;
        }

        public void SetChild(INode child)
        {
            _childNode = child;
        }
    }
}