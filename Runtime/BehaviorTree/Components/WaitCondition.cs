using System;
using UnityEngine;
using Zero53.BehaviorTree.Tree.Nodes.ConditionalNodes;

namespace Zero53.BehaviorTree.Components.ActionNodes
{
    [Serializable]
    public class WaitCondition : ICondition
    {
        public float duration;
        public float timer;
        
        public bool Evaluate()
        {
            timer += Time.deltaTime;
            if (timer < duration) return false;
            
            timer = 0;
            return true;
        }
    }
}