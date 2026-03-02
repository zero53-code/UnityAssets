using System.Collections.Generic;

namespace Zero53.BehaviorTree.CompositeNodes
{
    public class CompositeNode : Node
    {
        public CompositeNode(string name = "Node", int priority = 0, List<Node> children = null) : base(name, priority, children)
        {
        }
    }
}