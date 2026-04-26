using System;
using UnityEngine;

namespace Zero53.BehaviorTree.Tree.Nodes
{
    /// <summary>
    /// 叶子节点
    /// </summary>
    [Serializable]
    public class LeafNode : Node
    {
        [Serializable]
        public enum ExceptionHandling
        {
            Throw,
            ReturnSuccess,
            ReturnFailure,
        }
        
        [field: SerializeField]
        public ExceptionHandling exceptionHandling { get; set; } = ExceptionHandling.Throw;

        // protected LeafNode(string name = "Node", int priority = 0) : base(name, priority)
        // {
        // }

        public override void AddChild(Node child)
        {
            throw new NotSupportedException("无法向叶子节点中添加子节点");
        }
    }
}