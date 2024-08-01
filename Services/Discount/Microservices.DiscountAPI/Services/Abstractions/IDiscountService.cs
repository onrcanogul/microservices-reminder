using Microservices.DiscountAPI.Dtos;
using Microservices.DiscountAPI.Models;
using Microservices.Shared.Dtos;

namespace Microservices.DiscountAPI.Services.Abstractions
{
    public interface IDiscountService
    {
        Task<ServiceResponse<List<DiscountDto>>> GetDiscountsAsync();
        Task<ServiceResponse<DiscountDto>> GetDiscountByIdAsync(Guid id);
        Task<ServiceResponse<List<DiscountDto>>> GetUsersDiscountAsync(string userId);

        Task<ServiceResponse<DiscountDto>> ConfirmCodeAsync(string code, string userId); 
        Task<ServiceResponse<NoContent>> CreateDiscountAsync(CreateDiscountDto createDiscountDto);
        Task<ServiceResponse<NoContent>> UpdateDiscountAsync(UpdateDiscountDto updateDiscountDto);
        Task<ServiceResponse<NoContent>> DeleteDiscountAsync(Guid id);
    }
}
