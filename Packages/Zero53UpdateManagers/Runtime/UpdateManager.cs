using System.Collections.Generic;
using UnityEngine;

namespace Zero53.UpdateManagers
{
    public class UpdateManager : MonoBehaviour
    {
        private readonly List<IUpdateObserver> _observers = new();
        private readonly List<IUpdateObserver> _pendingObservers = new();
        
        private void Update()
        {
            foreach (var observer in _observers)
            {
                observer.ObservedUpdate();
            }
            
            _observers.AddRange(_pendingObservers);
            _pendingObservers.Clear();
        }

        public void RegisterObserver(IUpdateObserver observer)
        {
            _pendingObservers.Add(observer);
        }

        public void UnregisterObserver(IUpdateObserver observer)
        {
            _pendingObservers.Remove(observer);
            _observers.Remove(observer);
        }
    }
}