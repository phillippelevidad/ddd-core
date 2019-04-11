using System.Threading.Tasks;

namespace Core
{
    public interface IQueryDispatcher
    {
        Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query) where TResponse : IQueryResponse;
    }
}