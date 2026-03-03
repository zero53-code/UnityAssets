namespace Zero53.BehaviorTree
{
    public struct LeafNodeBase
    {
        private readonly NodeBase _base;
        public string name => _base.name;
        public int priority => _base.priority;
        
        public LeafNodeBase(string name = "Node", int priority = 0)
        {
            _base = new NodeBase(name, priority);
        }
    }
}