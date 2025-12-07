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
        public async Task<ValueResult<Page<AccountResponse>>> SearchAccounts(SearchAccountRequest request)
        {
            ValueResult<Page<AccountResponse>> result = await _queryMediator.QueryAsync(
                new SearchAccountsQuery
                {
                    FollowedBy = request.FollowedBy,
                    Following = request.Following,
                    SearchText = request.SearchQuery,
                    PageOptions = request.PageOptions,
                    SortOptions = request.SortOptions
                });

            return result;
        }

        [HttpGet("{uniqueName}")]
        public async Task<ValueResult<AccountResponse>> GetPublicAccountByUniqueName(string uniqueName)
        {
            ValueResult<AccountResponse> result = await _queryMediator.QueryAsync(
                new GetAccountByNameQuery(uniqueName)
                {
                    FollowedBy = User.GetOptionalAccountId()
                });

            return result;
        }

        [HttpGet("me"), Authorize, AllowUnverified]
        public async Task<ValueResult<AccountResponse>> GetDetailedAccount()
        {
            ValueResult<AccountResponse> result = await _queryMediator.QueryAsync(
                new GetAccountByIdQuery(User.GetAccountId()));

            return result;
        }

        [HttpPut, Authorize]
        public async Task<Result> UpdateAccount(UpdateAccountRequest request)
        {
            Result result = await _commandMediator.SendAsync(
                new UpdateAccountCommand(User.GetAccountId(), request.DisplayName)
                {
                    StatusText = request.StatusText
                });

            return result;
        }

        [HttpPost("avatar"), Authorize]
        public async Task<ValueResult<string>> UploadAvatar(IFormFile file)
        {
            ValueResult<string> result = await _commandMediator.SendAsync(
                new UploadAvatarCommand(User.GetAccountId(), file));

            return result;
        }

        [HttpDelete("avatar"), Authorize]
        public async Task<Result> DeleteAvatar()
        {
            Result result = await _commandMediator.SendAsync(
                new DeleteAvatarCommand(User.GetAccountId()));

            return result;
        }

        [HttpPost("{accountId:Guid}/follows"), Authorize]
        public async Task<Result> FollowAccount(Guid accountId)
        {
            Result result = await _commandMediator.SendAsync(
                new FollowAccountCommand(User.GetAccountId(), accountId));

            return result;
        }

        [HttpDelete("{accountId:Guid}/follows"), Authorize]
        public async Task<Result> UnfollowAccount(Guid accountId)
        {
            Result result = await _commandMediator.SendAsync(
                new UnfollowAccountCommand(User.GetAccountId(), accountId));

            return result;
        }
    }
}
