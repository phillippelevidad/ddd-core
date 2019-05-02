using MediatR;
using System.Threading.Tasks;

namespace Core
{
    public class MediatorQueryDispatcher : IQueryDispatcher
    {
        private readonly IMediator mediator;

        public MediatorQueryDispatcher(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query) where TResponse : IQueryResponse
        {
            return await mediator.Send(query);
        }
    }
}