using System;

namespace Zero53.BehaviorTree.Tree.Nodes.DecoratorNodes
{
    [Serializable]
    public abstract class DecoratorNode : Node
    {
        // protected DecoratorNode(string name = "Decorator", int priority = 0, Node child = null) : base(name, priority)
        // {
        //     if (child != null)
        //         children.Add(child);
        // }

        public override void AddChild(Node child)
        {
            if (children.Count >= 1) throw new NotSupportedException("装饰器节点有且只有一个子节点");
            
            base.AddChild(child);
        }
    }
}