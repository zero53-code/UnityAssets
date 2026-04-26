using System;
using System.Collections.Generic;
using System.Linq;
using Zero53.Utils;

namespace Zero53.BehaviorTree.Tree.Nodes.CompositeNodes
{
    [Serializable]
    public class RandomSequenceNode : PrioritySequenceNode
    {
        // public RandomSequenceNode(string name = "RandomSequence", int priority = 0, List<Node> children = null) : base(name, priority, children)
        // {
        // }
        
        protected override List<Node> SortChildren()
        {
            var sortChildren = children.ToList();
            sortChildren.Shuffle();
            return sortChildren;
        }
    }
}