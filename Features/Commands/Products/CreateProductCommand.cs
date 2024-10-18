using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Product;

namespace YssWebstoreApi.Features.Commands.Products
{
    public class CreateProductCommand : IRequest<ulong?>
    {
        public required ulong AccountId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string? SourceUrl { get; init; }
        public IEnumerable<Tag> Tags { get; init; } = [];

        public CreateProductCommand() { }

        [SetsRequiredMembers]
        public CreateProductCommand(ulong accountId, CreateProduct createProduct)
        {
            AccountId = accountId;
            Name = createProduct.Name;
            Description = createProduct.Description;
            SourceUrl = createProduct.SourceUrl;
            Tags = createProduct.Tags.Select(Tag.Parse);
        }
    }
}
