using Microservices.BasketAPI.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.BasketAPI.Services.Abstractions
{
    public interface IBasketService
    {
        Task<ServiceResponse<BasketDto>> GetBasketAsync(string id);
        Task<ServiceResponse<NoContent>> CreateOrUpdateAsync(BasketDto basketDto);
        Task<ServiceResponse<NoContent>> DeleteAsync(string userId);
    }
}
