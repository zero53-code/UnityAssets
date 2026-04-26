using Zero53.BehaviorTree.Tree.Nodes;
using Zero53.BehaviorTree.Tree.Nodes.CompositeNodes;

namespace Zero53.BehaviorTree.Components.ActionNodes
{
    public class BtSequence : BtNode
    {
        protected override Node BuildSelfNode()
        {
            return new SequenceNode();
        }
    }
}