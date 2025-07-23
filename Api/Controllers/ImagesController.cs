using LiteBus.Commands.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Resources;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Images.Commands;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ICommandMediator _mediator;

        public ImagesController(ICommandMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> UploadImage(UploadImage uploadImage)
        {
            if (!User.TryGetUserId(out var accountId))
            {
                return Unauthorized();
            }

            Result<ResourceResponse> resource = await _mediator.SendAsync(
                new UploadImageCommand(accountId, uploadImage.ImageFile));

            if (resource.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest(resource.Error);
        }
    }
}
