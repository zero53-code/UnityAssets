using System;

namespace Zero53.BehaviorTree.Tree.Nodes.DecoratorNodes
{
    /// <summary>
    /// 反转 Success 和 Failure
    /// </summary>
    [Serializable]
    public class InverterNode : DecoratorNode
    {
        // public InverterNode(string name = "Inverter", int priority = 0, Node child = null) : base(name, priority, child)
        // {
        // }

        protected override Status Process()
        {
            switch (children[0].ExecuteProcess())
            {
                case Status.Success: return Status.Failure;
                case Status.Failure: return Status.Success;
                case Status.Running:
                default: return Status.Running;
            }
        }
    }
}