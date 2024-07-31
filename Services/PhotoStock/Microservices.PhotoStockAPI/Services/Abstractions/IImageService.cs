using Microservices.Shared.Dtos;

namespace Microservices.PhotoStockAPI.Services.Abstractions
{
    public interface IImageService
    {
        Task<ServiceResponse<NoContent>> RemovePhoto(Guid imageId);
        Task<ServiceResponse<NoContent>> SavePhoto(IFormFileCollection photos, Guid productId);
    }
}
