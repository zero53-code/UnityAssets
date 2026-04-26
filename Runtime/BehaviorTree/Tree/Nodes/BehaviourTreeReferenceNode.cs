using System;

namespace Zero53.BehaviorTree.Tree.Nodes
{
    [Serializable]
    public class BehaviourTreeReferenceNode : LeafNode
    {
        [NonSerialized]
        public BehaviorTree behaviorTree;
        
        // public BehaviourTreeReferenceNode(BehaviorTree behaviorTree, string name = "BehaviourTreeReference", int priority = 0) : base(name, priority)
        // {
        //     _behaviorTree = behaviorTree;
        // }

        protected override Status Process()
        {
            return behaviorTree.ExecuteProcess();
        }

        public override void Reset()
        {
            base.Reset();
            behaviorTree.Reset();
        }
    }
}