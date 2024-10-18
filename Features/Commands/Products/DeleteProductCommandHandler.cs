using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Products
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ulong?>
    {
        private readonly IRepository<Product> _products;

        public DeleteProductCommandHandler(IRepository<Product> products)
        {
            _products = products;
        }

        public async Task<ulong?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _products.DeleteAsync(request.Id);
        }
    }
}
