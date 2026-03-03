using System;

namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 运行直到失败
    /// </summary>
    public class UntilFailNode : ISingleChildNode
    {
        private DecoratorNodeBase _base;
        public UntilFailNode(string name = "UntilFail", int priority = 0, INode child = null)
        {
            _base = new DecoratorNodeBase(name, priority, child);
            this.child = child;
        }
        
        public int priority => _base.nodeBase.priority;
        public NodeStatus Process()
        {
            if (child.Process() != NodeStatus.Failure) return NodeStatus.Running;
            
            Reset();
            return NodeStatus.Failure;
        }

        public void Reset()
        {
        }

        public INode child { get; set; }
    }
}