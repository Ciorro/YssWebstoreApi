using LiteBus.Commands.Abstractions;
using LiteBus.Messaging.Abstractions;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Entities;

namespace YssWebstoreApi.Features.Auth.Common
{
    public abstract class AuthenticatedCommand : ICommand
    {
        private const string AccountItemKey = "Account";

        public required string Email { get; set; }
        public required string Password { get; set; }

        public Account? AuthenticatedAccount
        {
            get => AmbientExecutionContext.Current.Items[AccountItemKey] as Account;
            set => AmbientExecutionContext.Current.Items[AccountItemKey] = value;
        }

        public bool IsAuthenticated()
        {
            return AmbientExecutionContext.Current.Items.ContainsKey(AccountItemKey);
        }

        public bool TryGetAuthenticatedAccount([NotNullWhen(true)] out Account account)
        {
            if (IsAuthenticated())
            {
                account = AuthenticatedAccount!;
                return true;
            }

            account = null!;
            return false;
        }
    }
}
