using YssWebstoreApi.Models;

namespace YssWebstoreApi.Services.Jwt
{
    public interface ITokenService
    {
        string GetJwt(Account account);
    }
}
