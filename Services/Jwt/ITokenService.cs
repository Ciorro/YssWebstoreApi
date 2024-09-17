using System.Security.Claims;
using YssWebstoreApi.Models;

namespace YssWebstoreApi.Services.Jwt
{
    public interface ITokenService
    {
        string GetJwt(params Claim[] claims);
    }
}
