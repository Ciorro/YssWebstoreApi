using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Commands.Accounts;
using YssWebstoreApi.Features.Queries.Accounts;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{accountId:int}")]
        public async Task<IActionResult> GetPublicAccount([FromRoute] ulong accountId)
        {
            var account = await _mediator.Send(new GetPublicAccountQuery(accountId));

            return account is PublicAccount ?
                Ok(account) :
                NotFound();
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetPrivateAccount()
        {
            try
            {
                var account = await _mediator.Send(new GetPrivateAccountQuery(User.GetUserId()));

                return account is PrivateAccount ?
                    Ok(account) :
                    NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccount updateAccountDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultId = await _mediator.Send(new UpdateAccountCommand(User.GetUserId(), updateAccountDTO));

            return resultId.HasValue ?
                Ok(resultId) :
                Problem();
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            var resultId = await _mediator.Send(new DeleteAccountCommand(User.GetUserId()));

            return resultId.HasValue ?
                Ok(resultId) :
                Problem();
        }
    }
}
