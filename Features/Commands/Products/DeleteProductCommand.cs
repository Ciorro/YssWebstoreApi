using MediatR;

namespace YssWebstoreApi.Features.Commands.Products
{
    public class DeleteProductCommand(ulong id) : IRequest<bool>
    {
        public ulong Id { get; } = id;
    }
}
