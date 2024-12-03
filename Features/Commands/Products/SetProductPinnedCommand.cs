using MediatR;

namespace YssWebstoreApi.Features.Commands.Products
{
    public class SetProductPinnedCommand(ulong productId) : IRequest<ulong?>
    {
        public ulong ProductId { get; } = productId;
        public required bool IsPinned { get; set; }
    }
}
