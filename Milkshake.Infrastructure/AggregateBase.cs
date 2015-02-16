using System;
using System.Collections.Generic;

namespace Milkshake.Infrastructure
{
    public class AggregateBase : IAggregate
    {
        public int Version
        {
            get
            {
                return _version;
            }
            protected set
            {
                _version = value;
            }
        }

        public Guid Id { get; protected set; }

        private List<IEvent> _uncommittedEvents = new List<IEvent>();
        private Dictionary<Type, Action<IEvent>> _routes = new Dictionary<Type, Action<IEvent>>();
        private int _version = -1;

        public void RaiseEvent(IEvent @event)
        {
            ApplyEvent(@event);
            _uncommittedEvents.Add(@event);
        }

        protected void RegisterTransition<T>(Action<T> transition) where T: class
        {
            _routes.Add(typeof(T), o => transition(o as T));
        }

        public void ApplyEvent(IEvent @event)
        {
            var eventType = @event.GetType();
            if (_routes.ContainsKey(eventType))
            {
                _routes[eventType](@event);
            }
            Version++;
        }

        public IEnumerable<IEvent> UncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }
    }
}
