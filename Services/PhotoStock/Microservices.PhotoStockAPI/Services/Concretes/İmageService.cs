using Microservices.PhotoStockAPI.Models;
using Microservices.PhotoStockAPI.Models.Contexts;
using Microservices.PhotoStockAPI.Services.Abstractions;
using Microservices.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Microservices.PhotoStockAPI.Services.Concretes
{
    public class İmageService(IWebHostEnvironment webHostEnvironment, ImageDbContext context) : IImageService
    {
        public async Task<ServiceResponse<NoContent>> SavePhoto(IFormFileCollection photos, Guid productId)
        {
            if (photos == null)
                return ServiceResponse<NoContent>.Failure("Photos are null", StatusCodes.Status400BadRequest);

            List<string> paths = await UploadAsync(photos);
            await context.Images.AddRangeAsync(paths.Select(p => new Models.Image
            {
                Path = p,
                ProductId = productId
            }).ToList());       
            await context.SaveChangesAsync();

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }
        public async Task<ServiceResponse<NoContent>> RemovePhoto(Guid imageId)
        {
            Image? image = await context.Images.FirstOrDefaultAsync(i => i.Id == imageId);
            if (image == null)
                return ServiceResponse<NoContent>.Failure("image not found", 404);

            context.Images.Remove(image);
            await context.SaveChangesAsync();

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }
        private async Task<bool> CopyFileAsync(string path, IFormFile image)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await image.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (IOException exception)
            {
                return false;
            }
        }
        private async Task<List<string>> UploadAsync(IFormFileCollection photos)
        {
            var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "Images");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<string> imagePaths = new();
            List<bool> results = new();

            foreach (var photo in photos)
            {
                bool result = await CopyFileAsync(Path.Combine(uploadPath,photo.FileName), photo);
                results.Add(result);
                imagePaths.Add(Path.Combine("Images", photo.FileName));
            }
            if(results.TrueForAll(r => r.Equals(true)))
                return imagePaths;

            throw new BadHttpRequestException("Bad Request for images");
        }
    }
}
