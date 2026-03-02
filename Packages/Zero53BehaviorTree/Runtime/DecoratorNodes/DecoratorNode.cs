using System;

namespace Zero53.BehaviorTree.DecoratorNodes
{
    public class DecoratorNode : Node
    {
        public DecoratorNode(string name = "Decorator", int priority = 0, Node child = null) : base(name, priority)
        {
            if (child != null)
                Children.Add(child);
        }

        public override void AddChild(Node child)
        {
            if (Children.Count >= 1) throw new NotSupportedException("装饰器节点有且只有一个子节点");
            
            base.AddChild(child);
        }
    }
}