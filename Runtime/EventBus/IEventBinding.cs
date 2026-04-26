using System;

namespace Zero53.EventBus
{
    public interface IEventBinding<T>
    {
        event Action<T> OnEvent;
        event Action OnEventNoArgs;
        void Invoke(T @event);
    }
}