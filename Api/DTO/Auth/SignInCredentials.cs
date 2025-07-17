using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Auth
{
    public class SignInCredentials
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? DeviceInfo { get; set; }
    }
}
