using System;
using System.Collections.Generic;
using System.Linq;
using Zero53.Utils;

namespace Zero53.BehaviorTree.Tree.Nodes.CompositeNodes
{
    [Serializable]
    public class RandomSelectorNode : PrioritySelectorNode
    {
        // public RandomSelectorNode(string name = "RandomSelector", int priority = 0, List<Node> children = null) : base(name, priority, children)
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