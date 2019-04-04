using MediatR;
using System.Threading.Tasks;

namespace Core
{
    public class MediatorQueryHandler : IQueryHandler
    {
        private readonly IMediator mediator;

        public MediatorQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query) where TResponse : IQueryResponse
        {
            return await mediator.Send(query);
        }
    }
}