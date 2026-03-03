using System.Collections.Generic;

namespace Zero53.BehaviorTree
{
    public readonly struct CompositeNodeBase
    {
        private readonly NodeBase _base;
        private readonly List<INode> _children;
        public string name => _base.name;
        public int priority => _base.priority;
        public int childCount => _children.Count;
        public IList<INode> children => _children;

        public CompositeNodeBase(string name = "Node", int priority = 0, List<INode> children = null)
        {
            _base = new NodeBase(name, priority);
            _children = children ?? new List<INode>();
        }

        public void AddChild(INode child)
        {
            _children.Add(child);
        }
        
        public void Reset()
        {
            for (var i = 0; i < _children.Count; i++)
            {
                _children[i].Reset();
            }
        }
    }
}