using System;

namespace Zero53.EventBus
{
    public class EventBinding<TEvent> : IEventBinding<TEvent> 
        where TEvent : IEvent
    {
        public event Action<TEvent> OnEvent;
        public event Action OnEventNoArgs;

        public EventBinding(Action<TEvent> onEvent, Action onEventNoArgs = null)
        {
            OnEvent = onEvent;
            OnEventNoArgs = onEventNoArgs;
        }

        public EventBinding(Action onEventNoArgs) : this(null, onEventNoArgs)
        {
        }

        public void Invoke(TEvent @event)
        {
            OnEvent?.Invoke(@event);
            OnEventNoArgs?.Invoke();
        }
    }
}