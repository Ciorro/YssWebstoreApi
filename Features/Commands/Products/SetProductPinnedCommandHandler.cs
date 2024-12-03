using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Products
{
    public class SetProductPinnedCommandHandler : IRequestHandler<SetProductPinnedCommand, ulong?>
    {
        private readonly IRepository<Product> _products;

        public SetProductPinnedCommandHandler(IRepository<Product> products)
        {
            _products = products;
        }

        public async Task<ulong?> Handle(SetProductPinnedCommand request, CancellationToken cancellationToken)
        {
            var product = await _products.GetAsync(request.ProductId);
            if (product is not null)
            {
                product.IsPinned = request.IsPinned;
                return await _products.UpdateAsync(product);
            }

            //TODO: return error
            return null;
        }
    }
}
