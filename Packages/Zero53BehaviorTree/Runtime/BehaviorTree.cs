namespace Zero53.BehaviorTree
{
    public class BehaviorTree
    {
        public string name { get; private set; }
        public INode root { get; private set; }
        
        public BehaviorTree(string name = "BehaviourTree", INode root = null)
        {
            this.name = name;
            this.root = root;
        }
        
        public NodeStatus Process()
        {
            return root?.Process() ?? NodeStatus.Failure;
        }

        public void Reset()
        {
            root?.Reset();
        }
    }
}