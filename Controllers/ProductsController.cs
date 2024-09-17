using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Mappers;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Product;
using YssWebstoreApi.Models.Query;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProduct([FromServices] IAccountRepository accountRepository, uint id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            var owner = await accountRepository.GetAsync(product.AccountId!.Value);
            var publicProduct = product.ToPublicProductDTO(owner?.ToPublicAccountDTO() ?? PublicAccount.Empty);

            return Ok(publicProduct);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> CreateProduct(CreateProduct createProductDTO)
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint accountId))
            {
                return Unauthorized();
            }

            var createdProduct = createProductDTO.ToProduct(accountId);

            if (await _productRepository.CreateAsync(createdProduct))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPut("{id:int}"), Authorize]
        public async Task<ActionResult> UpdateProduct(uint id, UpdateProduct updateProductDTO)
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint accountId))
            {
                return Unauthorized();
            }

            var updatedProduct = await _productRepository.GetAsync(id);
            if (updatedProduct is null)
            {
                return NotFound();
            }
            if (updatedProduct.AccountId != accountId)
            {
                return Unauthorized();
            }

            if (await _productRepository.UpdateAsync(id, updateProductDTO.ToProduct()))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}"), Authorize]
        public async Task<ActionResult> DeleteProduct(uint id)
        {
            if (!uint.TryParse(User.FindFirst("account_id")?.Value, out uint accountId))
            {
                return Unauthorized();
            }

            var deletedProduct = await _productRepository.GetAsync(id);
            if (deletedProduct is null)
            {
                return NotFound();
            }
            if (deletedProduct.AccountId != accountId)
            {
                return Unauthorized();
            }

            if (await _productRepository.DeleteAsync(id))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] SearchParams searchParams, [FromQuery] Pagination pagination, [FromQuery] SortParams sortParams)
        {
            return Ok(await _productRepository.GetAllAsync());
        }
    }
}
