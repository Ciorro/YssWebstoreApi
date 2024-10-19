using Microsoft.AspNetCore.Mvc;

namespace YssWebstoreApi.Controllers.Products
{
    [Route("api/products/{productId:int}/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProductImages(ulong productId)
        {

        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(ulong productId)
        {

        }

        [HttpDelete("{imageId:int}")]
        public async Task<IActionResult> DeleteImage(ulong productId, ulong imageId)
        {

        }
    }
}
