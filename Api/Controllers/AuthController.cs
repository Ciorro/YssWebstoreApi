﻿using LiteBus.Commands.Abstractions;
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
        private readonly ICommandMediator _mediator;

        public AuthController(ICommandMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpInformation signUpInfo)
        {
            Result<Guid> result = await _mediator.SendAsync(
                new CreateAccountCommand()
                {
                    Email = signUpInfo.Email,
                    Password = signUpInfo.Password,
                    UniqueName = signUpInfo.UniqueName,
                    DisplayName = signUpInfo.DisplayName
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return Conflict();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInCredentials signInCredentials)
        {
            Result<TokenCredentials> result = await _mediator.SendAsync(
                new CreateSessionCommand()
                {
                    Email = signInCredentials.Email,
                    Password = signInCredentials.Password,
                    DeviceInfo = signInCredentials.DeviceInfo,
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return Unauthorized();
        }

        [HttpPost("signout"), Authorize, AllowUnverified]
        public async Task<IActionResult> SignOutSession([FromBody] string sessionToken)
        {
            Result result = await _mediator.SendAsync(
                new DeleteSessionCommand(User.GetAccountId(), sessionToken));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("signout-all"), Authorize, AllowUnverified]
        public async Task<IActionResult> SignOutEverywhere()
        {
            Result result = await _mediator.SendAsync(
                new DeleteAllSessionsCommand(User.GetAccountId()));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(SignInSessionToken signInSessionToken)
        {
            Result<TokenCredentials> result = await _mediator.SendAsync(
                new UpdateSessionCommand(signInSessionToken.AccountId, signInSessionToken.SessionToken));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return Unauthorized();
        }
    }
}
