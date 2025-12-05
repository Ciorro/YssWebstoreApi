using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Api.Middlewares.Attributes;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Accounts.Commands;
using YssWebstoreApi.Features.Accounts.Queries;
using YssWebstoreApi.Features.Search.Queries;
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

        [HttpGet]
        public async Task<IActionResult> SearchAccounts(SearchAccountRequest request)
        {
            Result<Page<AccountResponse>> result = await _queryMediator.QueryAsync(
                new SearchAccountsQuery
                {
                    FollowedBy = request.FollowedBy,
                    Following = request.Following,
                    SearchText = request.SearchQuery,
                    PageOptions = request.PageOptions,
                    SortOptions = request.SortOptions
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpGet("{uniqueName}")]
        public async Task<IActionResult> GetPublicAccountByUniqueName(string uniqueName)
        {
            Result<AccountResponse> result = await _queryMediator.QueryAsync(
                new GetAccountByNameQuery(uniqueName)
                {
                    FollowedBy = User.GetOptionalAccountId()
                });

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

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateAccount(UpdateAccountRequest request)
        {
            Result result = await _commandMediator.SendAsync(
                new UpdateAccountCommand(User.GetAccountId(), request.DisplayName)
                {
                    StatusText = request.StatusText
                });

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("avatar"), Authorize]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            Result<string> result = await _commandMediator.SendAsync(
                new UploadAvatarCommand(User.GetAccountId(), file));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpDelete("avatar"), Authorize]
        public async Task<IActionResult> DeleteAvatar()
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteAvatarCommand(User.GetAccountId()));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("{accountId:Guid}/follows"), Authorize]
        public async Task<IActionResult> FollowAccount(Guid accountId)
        {
            Result result = await _commandMediator.SendAsync(
                new FollowAccountCommand(User.GetAccountId(), accountId));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{accountId:Guid}/follows"), Authorize]
        public async Task<IActionResult> UnfollowAccount(Guid accountId)
        {
            Result result = await _commandMediator.SendAsync(
                new UnfollowAccountCommand(User.GetAccountId(), accountId));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
