using MediatR;

namespace Core
{
    public interface IQuery<TResponse> : IRequest<TResponse> where TResponse : IQueryResponse
    {
    }
}