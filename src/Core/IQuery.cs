using MediatR;

namespace Core
{
    public interface IQuery<TResponse> : IRequest where TResponse : IQueryResponse
    {
    }
}