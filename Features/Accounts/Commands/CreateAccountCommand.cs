using LiteBus.Commands.Abstractions;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class CreateAccountCommand : ICommand<Result<Guid>>
    {
        public required string UniqueName { get; set; }
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
