using Microservices.BasketAPI.Dtos;
using Microservices.BasketAPI.Services.Abstractions;
using Microservices.Shared.Dtos;
using System.Text.Json;

namespace Microservices.BasketAPI.Services.Concretes
{
    public class BasketService(RedisService redisService) : IBasketService
    {
        public async Task<ServiceResponse<BasketDto>> GetBasketAsync(string id)
        {
            var existBasket = await redisService.GetDatabase().StringGetAsync(id);
            if (string.IsNullOrEmpty(existBasket))
                return ServiceResponse<BasketDto>.Failure("Basket not found", StatusCodes.Status404NotFound);

            return ServiceResponse<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), StatusCodes.Status200OK);

        }
        public async Task<ServiceResponse<NoContent>> CreateOrUpdate(BasketDto basketDto)
        {
            if(basketDto is null)
                return ServiceResponse<NoContent>.Failure("Basket is null", StatusCodes.Status400BadRequest);

            var status = await redisService.GetDatabase().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

            return status ? ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent) : ServiceResponse<NoContent>.Failure("Basket could not update or create", StatusCodes.Status500InternalServerError);

        }

        public async Task<ServiceResponse<NoContent>> Delete(string userId)
        {
            var existBasket = await redisService.GetDatabase().StringGetAsync(userId);

            if (string.IsNullOrEmpty(existBasket))
                return ServiceResponse<NoContent>.Failure("Basket not found", StatusCodes.Status404NotFound);

            var status = await redisService.GetDatabase().KeyDeleteAsync(userId);

            return status ? ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent) : ServiceResponse<NoContent>.Failure("Error while removing basket", StatusCodes.Status500InternalServerError);
        }

        
    }
}
