using System;

namespace Zero53.BehaviorTree.Tree.Nodes.DecoratorNodes
{
    /// <summary>
    /// 重复执行直到条件为 false
    /// </summary>
    [Serializable]
    public class RepeatNode : DecoratorNode
    {
        private readonly Func<bool> _stopCondition;
        
        // public RepeatNode(Func<bool> stopCondition, string name = "Repeat", int priority = 0, Node child = null) : base(name, priority, child)
        // {
        //     _stopCondition = stopCondition;
        // }

        protected override Status Process()
        {
            if (!_stopCondition()) return Status.Success;
            
            children[0].ExecuteProcess();
            return Status.Running;
        }
    }
}