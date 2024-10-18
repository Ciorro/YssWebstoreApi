using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Product;

namespace YssWebstoreApi.Features.Commands.Products
{
    public class UpdateProductCommand : IRequest<ulong?>
    {
        public required ulong ProductId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; } 
        public required string? SourceUrl { get; init; }

        public UpdateProductCommand() { }

        [SetsRequiredMembers]
        public UpdateProductCommand(ulong id, UpdateProduct updateProduct)
        {
            ProductId = id;
            Name = updateProduct.Name;
            Description = updateProduct.Description;
            SourceUrl = updateProduct.SourceUrl;
        }

    }
}
