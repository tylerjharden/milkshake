using System;
using System.Collections.Generic;

namespace Milkshake.Infrastructure
{
    public interface IAggregate
    {
        IEnumerable<IEvent> UncommittedEvents();
        void ClearUncommittedEvents();
        int Version { get; }
        Guid Id { get; }
        void ApplyEvent(IEvent @event);
    }
}
