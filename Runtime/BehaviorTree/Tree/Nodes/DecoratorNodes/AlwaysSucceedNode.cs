using System;

namespace Zero53.BehaviorTree.Tree.Nodes.DecoratorNodes
{
    /// <summary>
    /// 永远返回 Success
    /// </summary>
    [Serializable]
    public class AlwaysSucceedNode : DecoratorNode
    {
        // public AlwaysSucceedNode(string name = "AlwaysSucceed", int priority = 0, Node child = null) : base(name, priority, child)
        // {
        // }

        protected override Status Process()
        {
            return Status.Success;
        }
    }
}