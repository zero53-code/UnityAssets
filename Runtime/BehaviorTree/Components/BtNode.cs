using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.BehaviorTree.Tree.Nodes;
using Zero53.Utils;

namespace Zero53.BehaviorTree.Components
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public abstract class BtNode : MonoBehaviour
    {
        [BoxGroup("Debug", order: 10), ReadOnly] public Node node;
        [BoxGroup("Debug", order: 10), ShowInInspector, ReadOnly] public Status status = Status.None;

        public enum Status
        {
            Success,
            Failure,
            Running,
            None
        }
        
        public int priority;

#if !UNITY_EDITOR
        protected virtual void Awake()
        {
            gameObject.hideFlags = HideFlags.DontSave;
        }
#endif

        public Node BuildNode()
        {
            var selfNode = BuildSelfNode();
            selfNode.name = name;
            selfNode.priority = priority;
            
            selfNode.children = transform.GetChildren()
                .Select(child => child.GetComponent<BtNode>())
                .Where(child => child != this)
                .Select(child => child.BuildNode())
                .ToList();
            
#if UNITY_EDITOR
            node = selfNode;

            node.Succeeded += () => { status = Status.Success; };
            node.Failed += () => { status = Status.Failure; };
            node.Running += () => { status = Status.Running; };
            node.AfterReset += () => { status = Status.None; };
#endif
            
            return selfNode;
        }

        protected abstract Node BuildSelfNode();
    }
}