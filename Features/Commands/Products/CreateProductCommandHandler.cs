using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ulong?>
    {
        private readonly IRepository<Product> _products;

        public CreateProductCommandHandler(IRepository<Product> products)
        {
            _products = products;
        }

        public async Task<ulong?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return await _products.CreateAsync(new Product
            {
                AccountId = request.AccountId,
                Name = request.Name,
                Description = request.Description,
                SourceUrl = request.SourceUrl,
                Tags = request.Tags.ToArray()
            });
        }
    }
}
