using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Services.Abstractions;
using Microservices.Shared.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(ICourseService courseService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => ControllerResponse(await courseService.GetAllAsync());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id) 
            => ControllerResponse(await courseService.GetByIdAsync(id));
        [HttpGet("get-by-user-id/{userId}")]
        public async Task<IActionResult> GetByUser([FromRoute] string userId) 
            => ControllerResponse(await courseService.GetAllByUserAsync(userId));
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto model)
            => ControllerResponse(await courseService.CreateAsync(model));
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCourseDto model)
            => ControllerResponse(await courseService.UpdateAsync(model));
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
            => ControllerResponse(await courseService.DeleteAsync(id));

    }
}
