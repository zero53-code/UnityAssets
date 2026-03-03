using System;

namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 永远返回 Failure
    /// </summary>
    public class AlwaysFailNode : ISingleChildNode
    {
        private DecoratorNodeBase _base;
        public AlwaysFailNode(string name = "AlwaysFail", int priority = 0, INode child = null)
        {
            _base = new DecoratorNodeBase(name, priority, child);
            this.child = child;
        }

        public int priority => _base.nodeBase.priority;
        public NodeStatus Process()
        {
            return NodeStatus.Failure;
        }

        public void Reset()
        {
        }

        public INode child { get; set; }
    }
}