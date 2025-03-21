using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class CredentialsSignInQuery : IRequest<Account?>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        public CredentialsSignInQuery() { }

        [SetsRequiredMembers]
        public CredentialsSignInQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
