using Zero53.BehaviorTree.Tree.Nodes;
using Zero53.BehaviorTree.Tree.Nodes.DecoratorNodes;

namespace Zero53.BehaviorTree.Components.ActionNodes
{
    public class BtInverter : BtNode
    {
        protected override Node BuildSelfNode()
        {
            return new InverterNode();
        }
    }
}