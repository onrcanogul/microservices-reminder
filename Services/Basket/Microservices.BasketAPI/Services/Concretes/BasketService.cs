using Microservices.BasketAPI.Dtos;
using Microservices.BasketAPI.Services.Abstractions;
using Microservices.Shared.Dtos;
using Microservices.Shared.Exceptions;
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
        public async Task<ServiceResponse<NoContent>> CreateOrUpdateAsync(BasketDto basketDto)
        {
            if (basketDto is null)
                throw new BadRequestException("Basket is null or empty");

            var status = await redisService.GetDatabase().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

            if (!status)
                throw new InternalServerException("Basket could not update or create");

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }
        public async Task<ServiceResponse<NoContent>> DeleteAsync(string userId)
        {
            var existBasket = await redisService.GetDatabase().StringGetAsync(userId);

            if (string.IsNullOrEmpty(existBasket))
                throw new NotFoundException("Basket not found");

            var status = await redisService.GetDatabase().KeyDeleteAsync(userId);

            if (!status)
                throw new InternalServerException("Error while removing basket");

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }
    }
}
