using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Commands.Images;
using YssWebstoreApi.Features.Commands.Packages;
using YssWebstoreApi.Features.Queries.Images;
using YssWebstoreApi.Features.Queries.Products;
using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Image;

namespace YssWebstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateImage(CreateImage createImageDTO)
        {
            var resultId = await _mediator.Send(new CreateImageCommand(User.GetUserId(), createImageDTO));

            return resultId.HasValue ?
                Ok(resultId.Value) :
                Problem();
        }

        [HttpPut("{imageId:int}"), Authorize]
        public async Task<IActionResult> UpdateImage(ulong imageId, UpdateImage updateImageDTO)
        {
            var existingImage = await _mediator.Send(new GetImageByIdQuery(imageId));
            if (existingImage is null)
            {
                return NotFound();
            }

            if (existingImage.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new UpdateImageCommand(imageId, updateImageDTO));

            return resultId.HasValue ?
                Ok(resultId.Value) :
                Problem();
        }

        [HttpDelete("{imageId:int}"), Authorize]
        public async Task<IActionResult> DeleteImage(ulong imageId)
        {
            var existingImage = await _mediator.Send(new GetImageByIdQuery(imageId));
            if (existingImage is null)
            {
                return NotFound();
            }

            if (existingImage.Account?.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new DeleteImageCommand(imageId));
            return resultId ? Ok(resultId) : NotFound();
        }
    }
}
