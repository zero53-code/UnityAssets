using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zero53.EventBus
{
    public delegate void EventHandler<in TEvent>(TEvent e);
    
    public class EventBus<TPublisher, TEvent>
    {
        private readonly Dictionary<TPublisher, List<Delegate>> _handlers = new();

        public void Subscribe(TPublisher publisher, EventHandler<TEvent> handler)
        {
            if (!_handlers.TryGetValue(publisher, out var bindings))
            {
                bindings = new ();
                _handlers[publisher] = bindings;
            }
            bindings.Add(handler);
        }

        public void Unsubscribe(TPublisher publisher, EventHandler<TEvent> handler)
        {
            if (_handlers.TryGetValue(publisher, out var bindings))
            {
                bindings.Remove(handler);
            }
        }

        public void Publish(TPublisher publisher, TEvent @event)
        {
            if (!_handlers.TryGetValue(publisher, out var bindings) || bindings.Count == 0)
            {
                Debug.LogWarning($"发布者 '{publisher}' 的事件 '{@event}' 从未被订阅");
                return;
            }
            var eventBindings = bindings.ToArray();
            
            foreach (var handler in eventBindings)
            {
                ((EventHandler<TEvent>)handler).Invoke(@event);
            }
        }
    }
}