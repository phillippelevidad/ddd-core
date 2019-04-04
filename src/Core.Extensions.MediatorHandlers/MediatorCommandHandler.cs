using MediatR;
using System.Threading.Tasks;

namespace Core
{
    public class MediatorCommandHandler : ICommandHandler
    {
        private readonly IMediator mediator;

        public MediatorCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task HandleAsync(ICommand command)
        {
            await mediator.Publish(command);
        }
    }
}