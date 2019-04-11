using System.Threading.Tasks;

namespace Core
{
    public class MediatorQueryDispatcher : IQueryDispatcher
    {
        public Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query) where TResponse : IQueryResponse
        {
            throw new System.NotImplementedException();
        }
    }
}