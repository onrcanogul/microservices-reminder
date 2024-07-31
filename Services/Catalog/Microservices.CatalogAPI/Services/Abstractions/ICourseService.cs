using Microservices.CatalogAPI.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.CatalogAPI.Services.Abstractions
{
    public interface ICourseService
    {
        Task<ServiceResponse<NoContent>> CreateAsync(CreateCourseDto model);
        Task<ServiceResponse<NoContent>> DeleteAsync(string id);
        Task<ServiceResponse<List<CourseDto>>> GetAllAsync();
        Task<ServiceResponse<List<CourseDto>>> GetAllByUserAsync(string userId);
        Task<ServiceResponse<CourseDto>> GetByIdAsync(string id);
        Task<ServiceResponse<NoContent>> UpdateAsync(UpdateCourseDto model);
    }
}
