using System;
using UnityEngine;

namespace Zero53.Gas.Processes
{
    [Serializable]
    public sealed class PauseProcess<TProcess> : IProcess
        where TProcess : IProcess
    {
        [field: SerializeField] public TProcess process { get; private set; }
        [field: SerializeField] public bool isPaused { get; private set; }

        public PauseProcess(TProcess process)
        {
            this.process = process;
        }


        public void OnUpdate(float deltaTime)
        {
            if (isPaused) return;
            
            process.OnUpdate(deltaTime);
        }
    }
}