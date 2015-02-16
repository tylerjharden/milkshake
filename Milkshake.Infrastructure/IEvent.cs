using System;

namespace Milkshake.Infrastructure
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
