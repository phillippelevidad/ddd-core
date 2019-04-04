using System.Collections.Generic;

namespace Core
{
    public interface IPagedQueryResponse : IQueryResponse
    {
    }

    public interface IPagedQueryResponse<TItem> : IPagedQueryResponse
    {
        IEnumerable<TItem> Items { get; }
    }
}