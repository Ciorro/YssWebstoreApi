using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Models;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Test([FromServices] IRepository<Account> accounts)
        {
            var acc = new Account()
            {
                Id = Guid.CreateVersion7(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UniqueName = "tst",
                DisplayName = "Test",
                StatusText = "Zażółć gęślą jaźń",
                Credentials = new Credentials
                {
                    Id = Guid.CreateVersion7(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Email = "test@gmail.com",
                    PasswordHash = "ddd",
                    PasswordSalt = "aaa"
                },
                Sessions = [
                    new Session {
                        Id = Guid.CreateVersion7(),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        SessionToken = SecurityUtils.GetRandomString(255),
                    },
                    new Session {
                        Id = Guid.CreateVersion7(),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        SessionToken = SecurityUtils.GetRandomString(255),
                    }
                ]
            };

            await accounts.InsertAsync(acc);
            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Test2([FromServices] IRepository<Account> accounts, [FromRoute] Guid id)
        {
            var account = await accounts.GetAsync(id);
            
            account!.Sessions.Add(new Session
            {
                Id = Guid.CreateVersion7(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                SessionToken = SecurityUtils.GetRandomString(255),
                DeviceInfo = "Windows"
            });

            account.Sessions.First().UpdatedAt = new DateTime(2026, 1, 2);

            account.StatusText = "sus";

            account.Credentials.VerificationCode = "12345";
            
            await accounts.UpdateAsync(account!);

            return Ok(account);
        }

        [HttpGet("{id:guid}/del")]
        public async Task<IActionResult> Test22([FromServices] IRepository<Account> accounts, [FromRoute] Guid id)
        {
            await accounts.DeleteAsync(id);
            return Ok();
        }
    }
}
