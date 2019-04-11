using MediatR;

namespace Core
{
    public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent
    {
    }
}