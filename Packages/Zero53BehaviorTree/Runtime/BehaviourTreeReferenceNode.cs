namespace Zero53.BehaviorTree
{
    /// <summary>
    /// 行为树引用节点
    /// 将另一颗行为树作为节点
    /// </summary>
    public class BehaviourTreeReferenceNode : LeafNode
    {
        private readonly BehaviorTree _behaviorTree;
        
        public BehaviourTreeReferenceNode(BehaviorTree behaviorTree, string name = "BehaviourTreeReference", int priority = 0) : base(name, priority)
        {
            _behaviorTree = behaviorTree;
        }

        protected override Status Process()
        {
            return _behaviorTree.ExecuteProcess();
        }

        public override void Reset()
        {
            base.Reset();
            _behaviorTree.Reset();
        }
    }
}