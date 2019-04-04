namespace Core
{
    public interface IPagedQuery<TResponse> : IQuery<TResponse> where TResponse : IPagedQueryResponse
    {
        int StartIndex { get; }
        int PageSize { get; }
    }
}