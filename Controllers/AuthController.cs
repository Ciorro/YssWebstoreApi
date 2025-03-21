using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Commands.Auth;
using YssWebstoreApi.Features.Queries.Auth;
using YssWebstoreApi.Middlewares.Attributes;
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
            var account = await _mediator.Send(new CredentialsSignInQuery
            {
                Email = signInCredentials.Email,
                Password = signInCredentials.Password
            });

            if (account is null)
            {
                return Unauthorized();
            }

            var session = await _mediator.Send(new CreateSessionCommand(account));
            var accessToken = await _mediator.Send(new GetAccessTokenQuery(account));

            return Ok(new
            {
                AccessToken = accessToken,
                SessionToken = session!.SessionToken
            });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] CreateAccount createAccount)
        {
            var accountId = await _mediator.Send(new SignUpCommand(createAccount));

            return accountId.HasValue ?
                Ok(accountId) :
                Conflict();
        }


        [HttpPost("{accountId:int}/refresh")]
        public async Task<IActionResult> Refresh([FromRoute] ulong accountId, [FromBody] string sessionToken)
        {
            var account = await _mediator.Send(new SessionTokenSignInQuery
            {
                AccountId = accountId,
                SessionToken = sessionToken
            });

            if (account is null)
            {
                return Unauthorized();
            }

            var extendedSession = await _mediator.Send(new ExtendSessionCommand(account, sessionToken));
            if (extendedSession is not null)
            {
                return Ok(new
                {
                    AccessToken = await _mediator.Send(new GetAccessTokenQuery(account))
                });
            }

            return Problem();
        }

        [HttpPost("generateVerificationCode"), Authorize, AllowUnverified]
        public async Task<IActionResult> GenerateVerificationCode()
        {
            var result = await _mediator.Send(new GenerateVerificationCodeCommand(User.GetUserId()));

            return result ?
                Ok() :
                Problem();
        }

        [HttpPost("verify"), Authorize, AllowUnverified]
        public async Task<IActionResult> VerifyAccount([FromBody] string verificationCode)
        {
            var result = await _mediator.Send(new VerifyAccountCommand(User.GetUserId(), verificationCode));

            return result ?
                Ok() :
                Problem();
        }
    }
}
