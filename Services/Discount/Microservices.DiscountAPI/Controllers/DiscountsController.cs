using Microservices.DiscountAPI.Dtos;
using Microservices.DiscountAPI.Services.Abstractions;
using Microservices.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.DiscountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController(IDiscountService discountService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get()
            => ControllerResponse(await discountService.GetDiscountsAsync());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => ControllerResponse(await discountService.GetDiscountByIdAsync(id));

        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetByUser(string userId)
            => ControllerResponse(await discountService.GetUsersDiscountAsync(userId));
        [HttpGet("get-confirmed")]
        public async Task<IActionResult> GetConfirmed([FromQuery]string userId,[FromQuery] string code)
            => ControllerResponse(await discountService.GetConfirmedCodeAsync(code,userId));

        [HttpPost]
        public async Task<IActionResult> Create(CreateDiscountDto createDiscountDto)
            => ControllerResponse(await discountService.CreateDiscountAsync(createDiscountDto));
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDiscountDto updateDiscountDto)
            => ControllerResponse(await discountService.UpdateDiscountAsync(updateDiscountDto));
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
            => ControllerResponse(await discountService.DeleteDiscountAsync(id));

    }
}
