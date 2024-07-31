using Microservices.CatalogAPI.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.CatalogAPI.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<CategoryDto>>> GetAllAsync();
        Task<ServiceResponse<CategoryDto>> GetByIdAsync(string id);
        Task<ServiceResponse<NoContent>> CreateAsync(CreateCategoryDto model);
        Task<ServiceResponse<NoContent>> UpdateAsync(UpdateCategoryDto model);
        Task<ServiceResponse<NoContent>> DeleteAsync(string id);
    }
}
