using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Commands.Products;
using YssWebstoreApi.Features.Queries.Products;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.DTOs.Product;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(ulong id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));

            return result is PublicProduct ?
                Ok(result) :
                NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _mediator.Send(new GetAllProductsQuery()));
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct createProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultId = await _mediator.Send(new CreateProductCommand(User.GetUserId(), createProductDTO));

            return resultId.HasValue ? Ok(resultId) : Problem(
                "The input was valid but the server failed processing the request.");
        }

        [HttpPut("{id:int}"), Authorize]
        public async Task<IActionResult> UpdateProduct(ulong id, [FromBody] UpdateProduct updateProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product is null)
            {
                return NotFound();
            }

            if (product.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new UpdateProductCommand(id, updateProductDTO));

            return resultId.HasValue ?
                Ok(resultId) :
                Problem();
        }

        [HttpDelete("{id:int}"), Authorize]
        public async Task<IActionResult> DeleteProduct(ulong id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product is null)
            {
                return NotFound();
            }

            if (product.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new DeleteProductCommand(id));

            return resultId.HasValue ?
                Ok(resultId) :
                Problem();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] SearchProduct searchProduct,
            [FromQuery] SortOptions sortOptions,
            [FromQuery] PageOptions pageOptions)
        {
            return Ok(await _mediator.Send(new SearchProductsQuery(searchProduct)
            {
                SortOptions = sortOptions,
                PageOptions = pageOptions
            }));
        }
    }
}
