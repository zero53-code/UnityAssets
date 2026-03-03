using System;

namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 反转 Success 和 Failure
    /// </summary>
    public class InverterNode : ISingleChildNode
    {
        private DecoratorNodeBase _base;
        public InverterNode(string name = "Inverter", int priority = 0, INode child = null)
        {
            _base = new DecoratorNodeBase(name, priority, child);
            this.child = child;
        }

        public int priority => _base.nodeBase.priority;
        public NodeStatus Process()
        {
            return child.Process() switch
            {
                NodeStatus.Success => NodeStatus.Failure,
                NodeStatus.Failure => NodeStatus.Success,
                _ => NodeStatus.Running
            };
        }

        public void Reset()
        {
        }

        public INode child { get; set; }
    }
}