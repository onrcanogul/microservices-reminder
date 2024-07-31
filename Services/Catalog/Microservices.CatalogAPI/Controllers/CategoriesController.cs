using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Services.Abstractions;
using Microservices.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => ControllerResponse(await categoryService.GetAllAsync());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]string id)
            => ControllerResponse(await categoryService.GetByIdAsync(id));
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto model)
            => ControllerResponse(await categoryService.CreateAsync(model));
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto model)
            => ControllerResponse(await categoryService.UpdateAsync(model));
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
            => ControllerResponse(await categoryService.DeleteAsync(id));
    }
}
