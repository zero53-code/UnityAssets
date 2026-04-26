using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zero53.BehaviorTree.Tree.Nodes
{
    /// <summary>
    /// 行为树节点
    /// </summary>
    [Serializable]
    public class Node
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
        [Serializable]
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
        [SerializeField]
        public string name = "Node";
        
        /// <summary>
        /// 子节点
        /// </summary>
        [SerializeReference]
        public List<Node> children = new();

        /// <summary>
        /// 优先级
        /// </summary>
        [SerializeField]
        public int priority;

        /// <summary>
        /// 当前执行的子节点
        /// </summary>
        [SerializeField]
        protected int currentChild;

        public int childCount => children.Count;

        // protected Node(string name = "Node", int priority = 0, List<Node> children = null)
        // {
        //     this.name = name;
        //     this.priority = priority;
        //     this.children = children ?? new List<Node>();
        // }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(Node child)
        {
            children.Add(child);
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
            return children[currentChild].ExecuteProcess();
        }

        /// <summary>
        /// 递归重置节点和所有子节点
        /// </summary>
        public virtual void Reset()
        {
            currentChild = 0;
            foreach (var child in children)
            {
                child.Reset();
            }
            
            AfterReset?.Invoke();
        }
    }
}