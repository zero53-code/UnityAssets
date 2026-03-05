namespace Zero53.BehaviorTree
{
    /// <summary>
    /// 行为树引用节点
    /// 将另一颗行为树作为节点
    /// </summary>
    public class BehaviourTreeReferenceNode : INode
    {

        private readonly NodeBase _base;
        public BehaviorTree tree { get; set; }
        
        public BehaviourTreeReferenceNode(string name = "BehaviourTreeReference", int priority = 0, BehaviorTree tree = null)
        {
            _base = new NodeBase(name, priority);
            this.tree = tree;
        }

        public int priority => _base.priority;
        
        public NodeStatus Process()
        {
            return tree.Process();
        }

        public void Reset()
        {
            tree?.Reset();
        }
    }
}