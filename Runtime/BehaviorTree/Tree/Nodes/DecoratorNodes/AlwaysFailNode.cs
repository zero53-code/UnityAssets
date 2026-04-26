using System;

namespace Zero53.BehaviorTree.Tree.Nodes.DecoratorNodes
{
    /// <summary>
    /// 永远返回 Failure
    /// </summary>
    [Serializable]
    public class AlwaysFailNode : DecoratorNode
    {
        // public AlwaysFailNode(string name = "AlwaysFail", int priority = 0, Node child = null) : base(name, priority, child)
        // {
        // }

        protected override Status Process()
        {
            return Status.Failure;
        }
    }
}