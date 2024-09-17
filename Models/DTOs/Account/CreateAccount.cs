using YssWebstoreApi.Models.DTOs.Auth;

namespace YssWebstoreApi.Models.DTOs.Accounts
{
    public class CreateAccount
    {
        public required string UniqueName { get; init; }
        public required string DisplayName { get; init; }
        public required SignUpCredentials Credentials { get; init; }
    }
}
