using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Packages
{
    public class UpdatePackageRequest
    {
        [Length(3, 80)]
        public required string Name { get; set; }
    }
}
