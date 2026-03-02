using System.Collections.Generic;
using UnityEngine;

namespace Zero53.UpdateManagers
{
    public class FixedUpdateManager : MonoBehaviour
    {
        private readonly List<IFixedUpdateObserver> _observers = new();
        private readonly List<IFixedUpdateObserver> _pendingObservers = new();
        
        private void FixedUpdate()
        {
            foreach (var observer in _observers)
            {
                observer.ObservedFixedUpdate();
            }
            
            _observers.AddRange(_pendingObservers);
            _pendingObservers.Clear();
        }

        public void RegisterObserver(IFixedUpdateObserver observer)
        {
            _pendingObservers.Add(observer);
        }

        public void UnregisterObserver(IFixedUpdateObserver observer)
        {
            _pendingObservers.Remove(observer);
            _observers.Remove(observer);
        }
    }
}