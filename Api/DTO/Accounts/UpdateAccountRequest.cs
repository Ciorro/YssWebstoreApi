using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Accounts
{
    public class UpdateAccountRequest
    {
        [Length(1, 80)]
        public required string DisplayName { get; set; }

        [MaxLength(250)]
        public string? StatusText { get; set; }
    }
}
