namespace Zero53.BehaviorTree
{
    public struct NodeBase
    {
        /// <summary>
        /// 行为树节点名称
        /// </summary>
        public string name { get; private set;}

        /// <summary>
        /// 优先级
        /// </summary>
        public int priority { get; private set; }

        public NodeBase(string name = "Node", int priority = 0)
        {
            this.name = name;
            this.priority = priority;
        }
    }
}