using System;

namespace Zero53.BehaviorTree.ActionNodes
{
    public class ActionNode : INode
    {
        private LeafNodeBase _base;
        private Action callback { get; set; }

        public ActionNode(string name = "ActionNode", int priority = 0, Action callback = null)
        {
            _base = new LeafNodeBase(name, priority);
            this.callback = callback;
        }

        public int priority => _base.priority;

        public NodeStatus Process()
        {

            callback?.Invoke();
            return NodeStatus.Success;

        }

        public void Reset()
        {
        }
    }
}