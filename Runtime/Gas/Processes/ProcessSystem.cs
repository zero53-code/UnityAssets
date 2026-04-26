using System.Collections.Generic;

namespace Zero53.Gas.Buffs
{
    public class ProcessSystem : IProcess
    {
        private readonly HashSet<IProcess> _processes = new();
        private readonly HashSet<IProcess> _addPendingProcesses = new();
        private readonly HashSet<IProcess> _removePendingProcesses = new();

        public void OnUpdate(float deltaTime)
        {
            _processes.UnionWith(_addPendingProcesses);
            _addPendingProcesses.Clear();
            
            foreach (var process in _processes)
            {
                if (process is not IProcessEnd { isEnd: true }) continue;
                
                _removePendingProcesses.Add(process);

                if (process is not IProcessEndEvent p1) continue;
                
                p1.OnEnd();
            }
            _processes.ExceptWith(_removePendingProcesses);
            _removePendingProcesses.Clear();

            foreach (var process in _processes)
            {
                process.OnUpdate(deltaTime);
            }
        }

        public void AddProcess(IProcess buff)
        {
            _removePendingProcesses.Remove(buff);
            _addPendingProcesses.Add(buff);
        }
    }
}