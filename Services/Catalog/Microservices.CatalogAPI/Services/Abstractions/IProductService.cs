using Microservices.CatalogAPI.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.CatalogAPI.Services.Abstractions
{
    public interface IProductService
    {
        Task<ServiceResponse<NoContent>> CreateAsync(CreateProductDto model);
        Task<ServiceResponse<NoContent>> DeleteAsync(string id);
        Task<ServiceResponse<List<ProductDto>>> GetAllAsync();
        Task<ServiceResponse<List<ProductDto>>> GetAllByUserAsync(string userId);
        Task<ServiceResponse<ProductDto>> GetByIdAsync(string id);
        Task<ServiceResponse<NoContent>> UpdateAsync(UpdateProductDto model);
    }
}
