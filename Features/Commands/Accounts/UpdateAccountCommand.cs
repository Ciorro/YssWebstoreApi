using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class UpdateAccountCommand : IRequest<ulong?>
    {
        public required ulong AccountId { get; init; }
        public required string UniqueName { get; init; }
        public required string DisplayName { get; init; }
        public string? Status { get; init; }

        public UpdateAccountCommand() { }

        [SetsRequiredMembers]
        public UpdateAccountCommand(ulong accountId, UpdateAccount updateAccount)
        {
            AccountId = accountId;
            UniqueName = updateAccount.UniqueName;
            DisplayName = updateAccount.DisplayName;
            Status = updateAccount.Status;
        }
    }
}
