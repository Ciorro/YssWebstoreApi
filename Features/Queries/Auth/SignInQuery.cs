using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class SignInQuery : IRequest<(ulong accountId, string token)?>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        public SignInQuery() { }

        [SetsRequiredMembers]
        public SignInQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
