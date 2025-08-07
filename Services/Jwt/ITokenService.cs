using System.Security.Claims;

namespace YssWebstoreApi.Services.Jwt
{
    public interface ITokenService
    {
        string GetJwt(params Claim[] claims);
    }
}
