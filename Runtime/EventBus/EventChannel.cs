using UnityEngine;
using Zero53.EventBus;
using Zero53.Singletons;
using Object = UnityEngine.Object;

namespace Endfield.EventSystem
{
    [CreateAssetMenu(menuName = "Zero53/EventChannel", fileName = "New EventChannel")]
    public sealed class EventChannel : ScriptableObject
    {
#if UNITY_EDITOR

        [SerializeField, TextArea]
        private string comment;
        
#endif
        public void Subscribe<TEvent>(EventHandler<TEvent> handler)
        {
            Singleton<EventBus<EventChannelPublisher, TEvent>>
                .instance
                .Subscribe(new EventChannelPublisher(this), handler);
        }

        public void Unsubscribe<TEvent>(EventHandler<TEvent> handler)
        {
            Singleton<EventBus<EventChannelPublisher, TEvent>>
                .instance
                .Unsubscribe(new EventChannelPublisher(this), handler);
        }
        
        public void Publish<TEvent>(TEvent e)
        {
            Singleton<EventBus<EventChannelPublisher, TEvent>>
                .instance
                .Publish(new EventChannelPublisher(this), e);
        }
        
        public void Subscribe<TEvent>(Object publisher, EventHandler<TEvent> handler)
        {
            Singleton<EventBus<EventChannelPublisher, TEvent>>
                .instance
                .Subscribe(new EventChannelPublisher(this, publisher), handler);
        }

        public void Unsubscribe<TEvent>(Object publisher, EventHandler<TEvent> handler)
        {
            Singleton<EventBus<EventChannelPublisher, TEvent>>
                .instance
                .Unsubscribe(new EventChannelPublisher(this, publisher), handler);
        }
        
        public void Publish<TEvent>(Object publisher, TEvent e)
        {
            Singleton<EventBus<EventChannelPublisher, TEvent>>
                .instance
                .Publish(new EventChannelPublisher(this, publisher), e);
        }
    }
}