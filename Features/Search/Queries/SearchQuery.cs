using Dapper;
using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Search;

namespace YssWebstoreApi.Features.Search.Queries
{
    public abstract class SearchQuery<T> : IQuery<T>
    {
        public string? SearchText { get; init; }
        public PageOptions PageOptions { get; init; } = new();
        public SortOptions SortOptions { get; init; } = new();

        public abstract CommandDefinition GetCommandDefinition();
    }
}
