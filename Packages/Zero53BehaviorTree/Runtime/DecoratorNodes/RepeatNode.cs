using System;

namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 重复执行直到条件为 false
    /// </summary>
    public class RepeatNode : DecoratorNode
    {
        private readonly Func<bool> _stopCondition;
        
        public RepeatNode(Func<bool> stopCondition, string name = "Repeat", int priority = 0, Node child = null) : base(name, priority, child)
        {
            _stopCondition = stopCondition;
        }

        protected override Status Process()
        {
            if (!_stopCondition()) return Status.Success;
            
            Children[0].ExecuteProcess();
            return Status.Running;
        }
    }
}