using System.Threading;

namespace Core
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse> where TResponse : IQueryResponse
    {

    }
}