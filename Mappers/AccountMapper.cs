using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Mappers
{
    public static class AccountMapper
    {
        public static PublicAccount ToPublicAccountDTO(this Account account)
        {
            return new PublicAccount
            {
                Id = account.Id!.Value,
                CreatedAt = account.CreatedAt!.Value,
                UniqueName = account.UniqueName!,
                DisplayName = account.DisplayName!
            };
        }

        public static PrivateAccount ToPrivateAccountDTO(this Account account)
        {
            return new PrivateAccount
            {
                Id = account.Id!.Value,
                CreatedAt = account.CreatedAt!.Value,
                UpdatedAt = account.UpdatedAt!.Value,
                UniqueName = account.UniqueName!,
                DisplayName = account.DisplayName!
            };
        }

        public static Account ToAccount(this CreateAccount createAccountDTO)
        {
            return new Account
            {
                UniqueName = createAccountDTO.UniqueName,
                DisplayName = createAccountDTO.DisplayName,
            };
        }

        public static Account ToAccount(this UpdateAccount updateAccountDTO)
        {
            return new Account
            {
                UniqueName = updateAccountDTO.UniqueName,
                DisplayName = updateAccountDTO.DisplayName
            };
        }
    }
}
