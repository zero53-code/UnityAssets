using System;

namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 永远返回 Success
    /// </summary>
    public class AlwaysSucceedNode : ISingleChildNode
    {
        private DecoratorNodeBase _base;
        public AlwaysSucceedNode(string name = "AlwaysSucceed", int priority = 0, INode child = null)
        {
            _base = new DecoratorNodeBase(name, priority, child);
            this.child = child;
        }

        public int priority => _base.nodeBase.priority;
        public NodeStatus Process()
        {
            return NodeStatus.Success;
        }

        public void Reset()
        {
        }

        public INode child { get; set; }
    }
}