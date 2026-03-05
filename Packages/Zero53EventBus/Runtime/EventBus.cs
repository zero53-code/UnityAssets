using System.Collections.Generic;

namespace Zero53.EventBus
{
    public class EventBus<TPublisher, TEvent>
    {
        private readonly Dictionary<TPublisher, List<IEventBinding<TEvent>>> _handlers = new();

        public void Subscribe(TPublisher publisher, IEventBinding<TEvent> handler)
        {
            if (!_handlers.TryGetValue(publisher, out var bindings))
            {
                bindings = new List<IEventBinding<TEvent>>();
                _handlers[publisher] = bindings;
            }
            bindings.Add(handler);
        }

        public void Unsubscribe(TPublisher publisher, IEventBinding<TEvent> handler)
        {
            if (_handlers.TryGetValue(publisher, out var bindings))
            {
                bindings.Remove(handler);
            }
        }

        public void Publish(TPublisher publisher, TEvent @event)
        {
            if (!_handlers.TryGetValue(publisher, out var bindings)) return;
            var eventBindings = bindings.ToArray();
            
            foreach (var handler in eventBindings)
            {
                handler?.Invoke(@event);
            }
        }
    }
}