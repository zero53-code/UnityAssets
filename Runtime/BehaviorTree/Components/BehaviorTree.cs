using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.BehaviorTree.Tree.Nodes;

namespace Zero53.BehaviorTree.Components
{
    public class BehaviorTree : BtNode
    {
#if !UNITY_EDITOR
        protected override void Awake()
        {
        }
#endif

        public Tree.Nodes.BehaviorTree behaviorTree;

        private Coroutine _coroutine;
        
        [Button, HorizontalGroup]
        public void Run()
        {
            if (_coroutine != null) return;
            status = Status.Running;
            _coroutine = StartCoroutine(BehaviorTreeRunning());
        }

        [Button, HorizontalGroup]
        private void Build()
        {
            BuildNode();
        }

        [Button, HorizontalGroup, LabelText("Reset")]
        private void ResetBehaviorTree()
        {
            node.Reset();
        }
        
        private IEnumerator BehaviorTreeRunning()
        {
            while (status is Status.Running)
            {
                status = behaviorTree.ExecuteProcess() switch
                {
                    Node.Status.Running => Status.Running,
                    Node.Status.Success => Status.Success,
                    Node.Status.Failure => Status.Failure,
                    _ => Status.None
                };
                yield return null;
            }
            _coroutine = null;
        }
        
        protected override Node BuildSelfNode()
        {
            behaviorTree = new Tree.Nodes.BehaviorTree
            {
                name = name
            };
            
            return behaviorTree;
        }
    }
}