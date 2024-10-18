using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class SignUpCommand : IRequest<ulong?>
    {
        public required string UniqueName {  get; set; }
        public required string DisplayName {  get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public SignUpCommand() { }

        [SetsRequiredMembers]
        public SignUpCommand(CreateAccount account)
        {
            UniqueName = account.UniqueName;
            DisplayName = account.DisplayName;
            Email = account.Credentials.Email;
            Password = account.Credentials.Password;
        }
    }
}
