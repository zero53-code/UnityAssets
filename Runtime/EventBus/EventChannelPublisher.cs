using System;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace Arpg.EventSystem
{
    public class EventChannelPublisher : IEquatable<EventChannelPublisher>
    {
        public EventChannel channel { get; }

        [CanBeNull]
        public Object publisher { get; }

        public EventChannelPublisher(EventChannel channel, Object publisher = null)
        {
            this.publisher = publisher;
            this.channel = channel;
        }

        public override bool Equals(object obj)
        {
            return obj is EventChannelPublisher other && Equals(other);
        }

        public bool Equals(EventChannelPublisher other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(channel, other.channel) && Equals(publisher, other.publisher);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(channel, publisher);
        }
    }
}