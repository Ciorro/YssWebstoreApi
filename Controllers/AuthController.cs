using MediatR;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Features.Commands.Auth;
using YssWebstoreApi.Features.Queries.Auth;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Auth;

namespace YssWebstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInCredentials signInCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var signInResult = await _mediator.Send(new SignInQuery
            {
                Email = signInCredentials.Email,
                Password = signInCredentials.Password,
            });

            if (!signInResult.HasValue)
            {
                return Unauthorized();
            }

            var refreshToken = await _mediator.Send(
                new GetValidRefreshTokenQuery(signInResult.Value.accountId));
            refreshToken = await _mediator.Send(
                new GetProlongedRefreshTokenCommand(signInResult.Value.accountId)
                {
                    CurrentRefreshToken = refreshToken
                });

            return Ok(new
            {
                AccessToken = signInResult.Value.token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] CreateAccount createAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountId = await _mediator.Send(new SignUpCommand(createAccount));
            if (!accountId.HasValue)
            {
                return Conflict();
            }

            var accessToken = (await _mediator.Send(new SignInQuery
            {
                Email = createAccount.Credentials.Email,
                Password = createAccount.Credentials.Password
            }))?.token;

            var refreshToken = await _mediator.Send(new GetProlongedRefreshTokenCommand(accountId.Value));

            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized();
            }

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("{accountId:int}/refresh")]
        public async Task<IActionResult> Refresh([FromRoute] ulong accountId, [FromBody] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Unauthorized();
            }

            var tokenCredentials = await _mediator.Send(new TokenSignInQuery(accountId, refreshToken));
            if (tokenCredentials is null)
            {
                return Unauthorized();
            }

            await _mediator.Send(new GetProlongedRefreshTokenCommand(accountId)
            {
                CurrentRefreshToken = tokenCredentials.RefreshToken
            });

            return Ok(tokenCredentials!);
        }
    }
}
