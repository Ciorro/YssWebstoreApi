using MediatR;
using YssWebstoreApi.Services.Jwt;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class GetAccessTokenQueryHandler : IRequestHandler<GetAccessTokenQuery, string?>
    {
        private readonly ITokenService _tokenService;

        public GetAccessTokenQueryHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public Task<string?> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_tokenService.GetJwt(request.Account))!;
        }
    }
}
