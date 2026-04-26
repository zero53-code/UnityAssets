using System;

namespace Zero53.BehaviorTree.Tree.Nodes
{
    [Serializable]
    public class BehaviorTree : Node
    {
        // public BehaviorTree(string name = "BehaviourTree", Node child = null) : base(name)
        // {
        //     if (child != null)
        //         children.Add(child);
        // }
        
        public override void AddChild(Node child)
        {
            if (children.Count >= 1) throw new NotSupportedException();
            
            base.AddChild(child);
        }

        protected override Status Process()
        {
            return children[0].ExecuteProcess();
        }
    }
}