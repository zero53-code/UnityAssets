using System;

namespace Zero53.BehaviorTree
{
    public class BehaviorTree
    {
        private INode root { get; set; }
        
        public BehaviorTree(string name = "BehaviourTree", INode child = null)
        {
            root = child;
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