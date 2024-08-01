using Microservices.BasketAPI.Dtos;
using Microservices.BasketAPI.Services.Abstractions;
using Microservices.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController(IBasketService basketService) : CustomBaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetBasket(string userId)
            => ControllerResponse(await basketService.GetBasketAsync(userId));

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasket(BasketDto basketDto)
            => ControllerResponse(await basketService.CreateOrUpdateAsync(basketDto));

        [HttpDelete]
        public async Task<IActionResult> Delete(string basketId)
            => ControllerResponse(await basketService.DeleteAsync(basketId));
        

    }
}
