using System;

namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 重复执行直到条件为 false
    /// </summary>
    public class RepeatNode : ISingleChildNode
    {
        private DecoratorNodeBase _base;
        private readonly Func<bool> _stopCondition;
        
        public RepeatNode(Func<bool> stopCondition, string name = "Repeat", int priority = 0, INode child = null)
        {
            _stopCondition = stopCondition;
            _base = new DecoratorNodeBase(name, priority, child);
            this.child = child;
        }

        public int priority => _base.nodeBase.priority;
        public NodeStatus Process()
        {
            if (!_stopCondition()) return NodeStatus.Success;
            
            child.Process();
            return NodeStatus.Running;
        }

        public void Reset()
        {
        }

        public INode child { get; set; }
    }
}