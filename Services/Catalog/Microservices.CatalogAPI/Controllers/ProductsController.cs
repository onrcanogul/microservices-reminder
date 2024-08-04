using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Services.Abstractions;
using Microservices.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => ControllerResponse(await productService.GetAllAsync());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id) 
            => ControllerResponse(await productService.GetByIdAsync(id));
        [HttpGet("get-by-user-id/{userId}")]
        public async Task<IActionResult> GetByUser([FromRoute] string userId) 
            => ControllerResponse(await productService.GetAllByUserAsync(userId));
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto model)
            => ControllerResponse(await productService.CreateAsync(model));
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto model)
            => ControllerResponse(await productService.UpdateAsync(model));
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
            => ControllerResponse(await productService.DeleteAsync(id));

    }
}
