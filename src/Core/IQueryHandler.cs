using System.Threading.Tasks;

namespace Core
{
    public interface IQueryHandler
    {
        Task<TResponse> QueryAsync<TResponse>(IQuery<TResponse> query) where TResponse : IQueryResponse;
    }
}