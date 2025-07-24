using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.Middlewares.Attributes;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Accounts.Commands;
using YssWebstoreApi.Features.Accounts.Queries;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IQueryMediator _queryMediator;
        private readonly ICommandMediator _commandMediator;

        public AccountsController(IQueryMediator queryMediator, ICommandMediator commandMediator)
        {
            _queryMediator = queryMediator;
            _commandMediator = commandMediator;
        }

        [HttpGet("{uniqueName}")]
        public async Task<IActionResult> GetPublicAccountByUniqueName(string uniqueName)
        {
            Result<AccountResponse> result = await _queryMediator.QueryAsync(
                new GetAccountByNameQuery(uniqueName));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return NotFound();
        }

        [HttpGet("me"), Authorize, AllowUnverified]
        public async Task<IActionResult> GetDetailedAccount()
        {
            Result<AccountResponse> result = await _queryMediator.QueryAsync(
                new GetAccountByIdQuery(User.GetAccountId()));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return NotFound();
        }

        [HttpPost("verify"), Authorize, AllowUnverified]
        public async Task<IActionResult> Verify([FromBody] string verificationCode)
        {
            Result result = await _commandMediator.SendAsync(
                new VerifyAccountCommand(User.GetAccountId(), verificationCode));

            return result.Success ?
                Ok() : BadRequest();
        }

        [HttpPost("generate-verification-code"), Authorize, AllowUnverified]
        public async Task<IActionResult> GenerateVerificationCode()
        {
            Result result = await _commandMediator.SendAsync(
                new CreateVerificationCodeCommand(User.GetAccountId()));

            return result.Success ?
                Ok() : BadRequest();
        }
    }
}
