namespace Zero53.BehaviorTree.DecoratorNodes
{
    /// <summary>
    /// 反转 Success 和 Failure
    /// </summary>
    public class InverterNode : DecoratorNode
    {
        public InverterNode(string name = "Inverter", int priority = 0, Node child = null) : base(name, priority, child)
        {
        }

        protected override Status Process()
        {
            switch (Children[0].ExecuteProcess())
            {
                case Status.Success: return Status.Failure;
                case Status.Failure: return Status.Success;
                case Status.Running:
                default: return Status.Running;
            }
        }
    }
}