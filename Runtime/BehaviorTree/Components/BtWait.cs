using Sirenix.OdinInspector;
using UnityEngine;
using Zero53.BehaviorTree.Tree.Nodes;
using Zero53.BehaviorTree.Tree.Nodes.ConditionalNodes;
using Zero53.Utils;

namespace Zero53.BehaviorTree.Components.ActionNodes
{
    public class BtWait : BtNode, IProgress
    {
        [Min(0)] public float duration;
        [Min(0)] public float timer;

        [ShowInInspector, ProgressBar(min: 0, max: 1), PropertyOrder(-1)]
        public float progress => duration <= Mathf.Epsilon 
            ? 0f 
            : (timer / duration).Clamp01();

        protected override Node BuildSelfNode()
        {
            var waitCondition = new WaitCondition
            {
                duration = duration,
                timer = 0
            };
            
            var waitNode = new ConditionalRunningNode
            {
                condition = waitCondition
            };

#if UNITY_EDITOR
            waitNode.Running += () =>
            {
                duration = ((WaitCondition)waitNode.condition).duration;
                timer = ((WaitCondition)waitNode.condition).timer;
            };
#endif

            return waitNode;
        }
    }
}