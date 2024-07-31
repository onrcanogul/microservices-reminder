using Microservices.PhotoStockAPI.Services.Abstractions;
using Microservices.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.PhotoStockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController(IImageService imageService) : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> SavePhoto(IFormFileCollection images, Guid productId)
            => ControllerResponse(await imageService.SavePhoto(images, productId));
        [HttpDelete]
        public async Task<IActionResult> RemovePhoto(Guid imageId)
            => ControllerResponse(await imageService.RemovePhoto(imageId));
    }
}
