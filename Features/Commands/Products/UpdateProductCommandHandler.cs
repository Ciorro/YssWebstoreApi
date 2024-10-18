using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ulong?>
    {
        private readonly IRepository<Product> _products;

        public UpdateProductCommandHandler(IRepository<Product> products)
        {
            _products = products;
        }

        public async Task<ulong?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            return await _products.UpdateAsync(request.ProductId, new Product
            {
                Name = request.Name,
                Description = request.Description,
                SourceUrl = request.SourceUrl
            });
        }
    }
}
