using System.Collections.Generic;
using System.Linq;
using Zero53.Utils;

namespace Zero53.BehaviorTree.CompositeNodes
{
    public class RandomSequenceNode : PrioritySequenceNode
    {
        public RandomSequenceNode(string name = "RandomSequence", int priority = 0, List<Node> children = null) : base(name, priority, children)
        {
        }
        
        protected override List<Node> SortChildren()
        {
            var children = Children.ToList();
            children.Shuffle();
            return children;
        }
    }
}