using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Package;
using YssWebstoreApi.Models.DTOs.Product;

namespace YssWebstoreApi.Mappers
{
    public static class ProductMapper
    {
        public static PublicProduct ToPublicProductDTO(this Product product)
        {
            return new PublicProduct
            {
                Id = product.Id,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                AccountId = product.AccountId!.Value,
                Name = product.Name!,
                Description = product.Description!,
                SourceUrl = product.SourceUrl!
            };
        }

        public static Product ToProduct(this CreateProduct createProductDTO, uint accountId)
        {
            return new Product
            {
                AccountId = accountId,
                Name = createProductDTO.Name,
                Description = createProductDTO.Description,
                SourceUrl = createProductDTO.SourceUrl
            };
        }

        public static Product ToProduct(this UpdateProduct updateProductDTO)
        {
            return new Product
            {
                Name = updateProductDTO.Name,
                Description = updateProductDTO.Description,
                SourceUrl = updateProductDTO.SourceUrl
            };
        }
    }
}
