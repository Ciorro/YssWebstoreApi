using LiteBus.Commands;
using LiteBus.Commands.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Auth;
using YssWebstoreApi.Api.Middlewares.Attributes;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Accounts.Commands;
using YssWebstoreApi.Features.Sessions.Commands;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICommandMediator _commandMediator;

        public AuthController(ICommandMediator mediator)
        {
            _commandMediator = mediator;
        }


        [HttpPost("signup")]
        public async Task<ValueResult<Guid>> SignUp(SignUpInformation signUpInfo)
        {
            ValueResult<Guid> result = await _commandMediator.SendAsync(
                new CreateAccountCommand()
                {
                    Email = signUpInfo.Email,
                    Password = signUpInfo.Password,
                    UniqueName = signUpInfo.UniqueName,
                    DisplayName = signUpInfo.DisplayName
                });

            return result;
        }

        [HttpPost("signin")]
        public async Task<ValueResult<TokenCredentials>> SignIn(SignInCredentials signInCredentials)
        {
            ValueResult<TokenCredentials> result = await _commandMediator.SendAsync(
                new CreateSessionCommand()
                {
                    Email = signInCredentials.Email,
                    Password = signInCredentials.Password,
                    DeviceInfo = signInCredentials.DeviceInfo,
                });

            return result;
        }

        [HttpPost("signout"), Authorize, AllowUnverified]
        public async Task<Result> SignOutSession([FromBody] string sessionToken)
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteSessionCommand(User.GetAccountId(), sessionToken));

            return result;
        }

        [HttpPost("signout-all"), Authorize, AllowUnverified]
        public async Task<Result> SignOutEverywhere()
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteAllSessionsCommand(User.GetAccountId()));

            return result;
        }

        [HttpPost("refresh")]
        public async Task<ValueResult<TokenCredentials>> Refresh(SignInSessionToken signInSessionToken)
        {
            ValueResult<TokenCredentials> result = await _commandMediator.SendAsync(
                new UpdateSessionCommand(signInSessionToken.AccountId, signInSessionToken.SessionToken));

            return result;
        }

        [HttpPost("verify"), Authorize, AllowUnverified]
        public async Task<Result> Verify([FromBody] string verificationCode)
        {
            Result result = await _commandMediator.SendAsync(
                new VerifyAccountCommand(User.GetAccountId(), verificationCode));

            return result;
        }

        [HttpPost("generate-verification-code"), Authorize, AllowUnverified]
        public async Task<Result> GenerateVerificationCode()
        {
            Result result = await _commandMediator.SendAsync(
                new CreateVerificationCodeCommand(User.GetAccountId()));

            return result;
        }
    }
}
