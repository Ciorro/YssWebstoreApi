using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Package;

namespace YssWebstoreApi.Mappers
{
    public static class PackageMapper
    {
        public static PublicPackage ToPublicPackage(this Package package)
        {
            return new PublicPackage
            {
                Id = package.Id!.Value,
                CreatedAt = package.CreatedAt!.Value,
                UpdatedAt = package.UpdatedAt!.Value,
                ProductId = package.ProductId!.Value,
                FileSize = package.FileSize!.Value,
                Name = package.Name!,
                Version = package.Version!,
                DownloadUrl = package.DownloadUrl!,
                TargetOS = package.TargetOS,
            };
        }

        public static Package ToPackage(this CreatePackage createPackageDTO)
        {
            return new Package
            {
                ProductId = createPackageDTO.ProductId,
                Name = createPackageDTO.Name,
                Version = createPackageDTO.Version,
                DownloadUrl = createPackageDTO.DownloadUrl,
                TargetOS = createPackageDTO.TargetOS
            };
        }

        public static Package ToPackage(this UpdatePackage updatePackageDTO)
        {
            return new Package
            {
                Name = updatePackageDTO.Name
            };
        }
    }
}
