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
            var product = await _products.GetAsync(request.ProductId);
            if (product is not null)
            {
                product.Name = request.Name;
                product.Description = request.Description;
                product.SourceUrl = request.SourceUrl;

                return await _products.UpdateAsync(product);
            }

            //TODO: return error
            return null;
        }
    }
}
