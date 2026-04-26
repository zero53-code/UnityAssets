using System;

namespace Zero53.BehaviorTree.Tree.Nodes.DecoratorNodes
{
    /// <summary>
    /// 运行直到失败
    /// </summary>
    [Serializable]
    public class UntilFailNode : DecoratorNode
    {
        // public UntilFailNode(string name = "UntilFail", int priority = 0, Node child = null) : base(name, priority, child)
        // {
        // }

        protected override Status Process()
        {
            if (children[0].ExecuteProcess() != Status.Failure) return Status.Running;
            
            Reset();
            return Status.Failure;

        }
    }
}