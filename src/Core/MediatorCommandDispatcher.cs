using MediatR;
using System.Threading.Tasks;

namespace Core
{
    public class MediatorCommandDispatcher : ICommandDispatcher
    {
        private readonly IMediator mediator;

        public MediatorCommandDispatcher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task DispatchAsync(ICommand command)
        {
            await mediator.Publish(command);
        }
    }
}