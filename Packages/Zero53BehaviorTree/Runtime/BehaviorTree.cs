using System;

namespace Zero53.BehaviorTree
{
    public class BehaviorTree : Node
    {
        public BehaviorTree(string name = "BehaviourTree", Node child = null) : base(name)
        {
            if (child != null)
                Children.Add(child);
        }
        
        public override void AddChild(Node child)
        {
            if (Children.Count >= 1) throw new NotSupportedException();
            
            base.AddChild(child);
        }

        protected override Status Process()
        {
            return Children[0].ExecuteProcess();
        }
    }
}