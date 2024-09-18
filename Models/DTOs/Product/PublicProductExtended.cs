using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Package;

namespace YssWebstoreApi.Models.DTOs.Product
{
    public class PublicProductExtended : PublicProduct
    {
        public required PublicAccount Account { get; set; }
        public required IEnumerable<PublicPackage> Packages{ get; set; }
    }
}
