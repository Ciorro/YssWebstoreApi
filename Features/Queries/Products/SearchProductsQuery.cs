using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.DTOs.Product;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Features.Queries.Products
{
    public class SearchProductsQuery : IRequest<Page<PublicProduct>>
    {
        public required SearchProduct SearchParams { get; init; }
        public SortOptions SortOptions { get; init; } = new();
        public PageOptions PageOptions { get; init; } = new();

        public SearchProductsQuery() { }

        [SetsRequiredMembers]
        public SearchProductsQuery(SearchProduct searchParams)
        {
            SearchParams = searchParams;
        }
    }
}
