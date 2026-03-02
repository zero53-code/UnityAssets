using System.Collections.Generic;
using UnityEngine;

namespace Zero53.UpdateManagers
{
    public class LateUpdateManager : MonoBehaviour
    {
        private readonly List<ILateUpdateObserver> _observers = new();
        private readonly List<ILateUpdateObserver> _pendingObservers = new();
        
        private void LateUpdate()
        {
            foreach (var observer in _observers)
            {
                observer.ObservedLateUpdate();
            }
            
            _observers.AddRange(_pendingObservers);
            _pendingObservers.Clear();
        }

        public void RegisterObserver(ILateUpdateObserver observer)
        {
            _pendingObservers.Add(observer);
        }

        public void UnregisterObserver(ILateUpdateObserver observer)
        {
            _pendingObservers.Remove(observer);
            _observers.Remove(observer);
        }
    }
}