using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Mappers;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Package;
using YssWebstoreApi.Models.DTOs.Product;
using YssWebstoreApi.Repositories;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageRepository _packageRepository;

        public PackagesController(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetPackage(uint id)
        {
            var package = await _packageRepository.GetAsync(id);
            if (package is null)
            {
                return NotFound();
            }

            return Ok(package.ToPublicPackage());
        }

        [HttpGet("product/{id:int}")]
        public async Task<ActionResult> GetPackagesByProduct(uint id)
        {
            var packages = await _packageRepository.GetPackagesByProductAsync(id);
            if (packages is null)
            {
                return NotFound();
            }

            return Ok(packages.Select(x => x.ToPublicPackage()));
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> CreatePackage([FromServices] IProductRepository productRepository, CreatePackage createPackageDTO)
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint accountId))
            {
                return Unauthorized();
            }

            var product = await productRepository.GetAsync(createPackageDTO.ProductId);
            if (product is null)
            {
                return NotFound();
            }
            if (product.AccountId != accountId)
            {
                return Unauthorized();
            }

            if (await _packageRepository.CreateAsync(createPackageDTO.ToPackage()))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPut("{id:int}"), Authorize]
        public async Task<ActionResult> UpdatePackage([FromServices] IProductRepository productRepository, uint id, UpdatePackage updatePackageDTO)
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint accountId))
            {
                return Unauthorized();
            }

            var updatedPackage = await _packageRepository.GetAsync(id);
            if (updatedPackage is null)
            {
                return NotFound();
            }

            var product = await productRepository.GetAsync(updatedPackage.ProductId!.Value);
            if (product is null)
            {
                return NotFound();
            }
            if (product.AccountId != accountId)
            {
                return Unauthorized();
            }

            if (await _packageRepository.UpdateAsync(id, updatePackageDTO.ToPackage()))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}"), Authorize]
        public async Task<ActionResult> DeletePackage([FromServices] IProductRepository productRepository, uint id)
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint accountId))
            {
                return Unauthorized();
            }

            var deletedPackage = await _packageRepository.GetAsync(id);
            if (deletedPackage is null)
            {
                return NotFound();
            }

            var product = await productRepository.GetAsync(deletedPackage.ProductId!.Value);
            if (product?.AccountId != accountId)
            {
                return Unauthorized();
            }

            if (await _packageRepository.DeleteAsync(id))
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
