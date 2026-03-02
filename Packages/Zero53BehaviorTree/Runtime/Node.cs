using System;
using System.Collections.Generic;

namespace Zero53.BehaviorTree
{
    /// <summary>
    /// 行为树节点
    /// </summary>
    public class Node : IAddChildNode
    {
        public event Action Enter;
        public event Action Exit;

        public event Action Succeeded;
        public event Action Failed;
        public event Action Running;
        public event Action AfterReset;
        
        /// <summary>
        /// 行为树节点执行状态
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// 成功
            /// </summary>
            Success,
            /// <summary>
            /// 失败
            /// </summary>
            Failure,
            /// <summary>
            /// 运行中
            /// </summary>
            Running
        }

        /// <summary>
        /// 行为树节点名称
        /// </summary>
        public readonly string Name;
        
        public int childCount => Children.Count;

        /// <summary>
        /// 子节点
        /// </summary>
        protected readonly List<Node> Children;

        /// <summary>
        /// 优先级
        /// </summary>
        public readonly int Priority;

        /// <summary>
        /// 当前执行的子节点
        /// </summary>
        protected int CurrentChild;

        protected Node(string name = "Node", int priority = 0, List<Node> children = null)
        {
            Name = name;
            Priority = priority;
            Children = children ?? new List<Node>();
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(Node child)
        {
            Children.Add(child);
        }

        /// <summary>
        /// 执行过程
        /// </summary>
        /// <returns>执行状态</returns>
        public Status ExecuteProcess()
        {
            Enter?.Invoke();
            var result = Process();

            switch (result)
            {
                case Status.Success:
                    Succeeded?.Invoke();
                    break;
                case Status.Failure:
                    Failed?.Invoke();
                    break;
                case Status.Running:
                default:
                    Running?.Invoke();
                    break;
            }
            
            Exit?.Invoke();
            
            return result;
        }
        
        /// <summary>
        /// 过程
        /// </summary>
        /// <returns>状态</returns>
        protected virtual Status Process()
        {
            return Children[CurrentChild].ExecuteProcess();
        }

        /// <summary>
        /// 递归重置节点和所有子节点
        /// </summary>
        public virtual void Reset()
        {
            CurrentChild = 0;
            foreach (var child in Children)
            {
                child.Reset();
            }
            
            AfterReset?.Invoke();
        }
    }
}