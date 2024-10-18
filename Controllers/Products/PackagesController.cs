using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Commands.Packages;
using YssWebstoreApi.Features.Queries.Packages;
using YssWebstoreApi.Features.Queries.Products;
using YssWebstoreApi.Models.DTOs.Package;

namespace YssWebstoreApi.Controllers
{
    [Route("api/products/{productId:int}/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PackagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPackages(ulong productId)
        {
            return Ok(await _mediator.Send(new GetPackagesByProductIdQuery(productId)));
        }

        [HttpGet("{packageId:int}")]
        public async Task<ActionResult> GetPackage(ulong productId, ulong packageId)
        {
            var package = (await _mediator.Send(new GetPackagesByProductIdQuery(productId)))
                .SingleOrDefault(x => x.Id == packageId);

            return package is PublicPackage ?
                Ok(package) :
                NotFound();
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> CreatePackage(ulong productId, CreatePackage createPackageDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parentProduct = await _mediator.Send(new GetProductByIdQuery(productId));
            if (parentProduct?.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new CreatePackageCommand(productId, createPackageDTO));

            return resultId.HasValue ?
                Ok(resultId.Value) :
                Problem();
        }

        [HttpPut("{packageId:int}"), Authorize]
        public async Task<ActionResult> UpdatePackage(ulong productId, ulong packageId, UpdatePackage updatePackageDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var parentProduct = await _mediator.Send(new GetProductByIdQuery(productId));
            if (parentProduct?.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new UpdatePackageCommand(packageId, updatePackageDTO));

            return resultId.HasValue ?
                Ok(resultId) :
                NotFound();
        }

        [HttpDelete("{packageId:int}"), Authorize]
        public async Task<ActionResult> DeletePackage(ulong productId, ulong packageId)
        {
            var parentProduct = await _mediator.Send(new GetProductByIdQuery(productId));
            if (parentProduct?.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new DeletePackageCommand(packageId));

            return resultId.HasValue ?
                Ok(resultId) :
                NotFound();
        }
    }
}
