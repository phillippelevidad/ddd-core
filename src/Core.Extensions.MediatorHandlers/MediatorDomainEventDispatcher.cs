using MediatR;
using System.Threading.Tasks;

namespace Core
{
    public class MediatorDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator mediator;

        public MediatorDomainEventDispatcher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task DispatchAsync(IDomainEvent domainEvent)
        {
            await mediator.Publish(domainEvent);
        }
    }
}