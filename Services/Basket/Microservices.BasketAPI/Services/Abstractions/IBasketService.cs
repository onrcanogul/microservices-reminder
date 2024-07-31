using Microservices.BasketAPI.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.BasketAPI.Services.Abstractions
{
    public interface IBasketService
    {
        Task<ServiceResponse<BasketDto>> GetBasketAsync(string id);
        Task<ServiceResponse<NoContent>> CreateOrUpdate(BasketDto basketDto);
        Task<ServiceResponse<NoContent>> Delete(string userId);
    }
}
