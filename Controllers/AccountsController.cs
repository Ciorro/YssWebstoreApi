using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Mappers;
using YssWebstoreApi.Middlewares.Attributes;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet("{id:int}/public")]
        public async Task<IActionResult> GetPublicAccount([FromRoute] uint id)
        {
            var account = await _accountRepository.GetAsync(id);

            if (account is null)
            {
                return NotFound();
            }

            return Ok(account.ToPublicAccountDTO());
        }

        [HttpGet("private"), Authorize]
        public async Task<IActionResult> GetPrivateAccount()
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint id))
            {
                return Unauthorized();
            }

            var account = await _accountRepository.GetAsync(id);

            if (account is null)
            {
                return NotFound();
            }

            return Ok(account.ToPrivateAccountDTO());
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccount updateAccountDTO)
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint id))
            {
                return Unauthorized();
            }

            if (await _accountRepository.UpdateAsync(id, updateAccountDTO.ToAccount()))
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint id))
            {
                return Unauthorized();
            }

            if (await _accountRepository.DeleteAsync(id))
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
