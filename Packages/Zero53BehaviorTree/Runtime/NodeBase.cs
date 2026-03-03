using System;

namespace Zero53.BehaviorTree
{
    // /// <summary>
    // /// 行为树节点
    // /// </summary>
    // public class Node : ICompositeNode, INode
    // {
    //     public event Action Enter;
    //     public event Action Exit;
    //
    //     public event Action Succeeded;
    //     public event Action Failed;
    //     public event Action Running;
    //     public event Action ResetAfter;
    //
    //     private CompositeNodeBase _base;
    //
    //     protected Node(string name = "Node", int priority = 0, List<INode> children = null)
    //     {
    //         _base = new CompositeNodeBase(name, priority, children);
    //     }
    //
    //     /// <summary>
    //     /// 添加子节点
    //     /// </summary>
    //     /// <param name="child"></param>
    //     public virtual void AddChild(Node child)
    //     {
    //         _base.AddChild(child);
    //     }
    //
    //     /// <summary>
    //     /// 执行过程
    //     /// </summary>
    //     /// <returns>执行状态</returns>
    //     public NodeStatus ExecuteProcess()
    //     {
    //         Enter?.Invoke();
    //         var result = Process();
    //
    //         switch (result)
    //         {
    //             case NodeStatus.Success:
    //                 Succeeded?.Invoke();
    //                 break;
    //             case NodeStatus.Failure:
    //                 Failed?.Invoke();
    //                 break;
    //             case NodeStatus.Running:
    //             default:
    //                 Running?.Invoke();
    //                 break;
    //         }
    //         
    //         Exit?.Invoke();
    //         
    //         return result;
    //     }
    //     
    //     /// <summary>
    //     /// 过程
    //     /// </summary>
    //     /// <returns>状态</returns>
    //     public NodeStatus Process()
    //     {
    //         return _base.Process();
    //     }
    //
    //     /// <summary>
    //     /// 递归重置节点和所有子节点
    //     /// </summary>
    //     public void Reset()
    //     {
    //         _base.Reset();
    //         
    //         ResetAfter?.Invoke();
    //     }
    // }

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