using AutoMapper;
using MassTransit;
using Microservices.CatalogAPI.Configurations;
using Microservices.CatalogAPI.Contexts;
using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Models;
using Microservices.CatalogAPI.Services.Abstractions;
using Microservices.Shared.Dtos;
using Microservices.Shared.Events;
using Microservices.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Text.Json;

namespace Microservices.CatalogAPI.Services.Concretes
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly CatalogOutboxDbContext _dbContext;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings,CatalogOutboxDbContext dbContext ,IPublishEndpoint publishEndpoint)
        {
            MongoClient mongoClient = new(databaseSettings.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(databaseSettings.CourseCollectionName);
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
        }
        public async Task<ServiceResponse<List<ProductDto>>> GetAllAsync()
        {

            List<Product> products = await (await _productCollection.FindAsync(product => true)).ToListAsync();

            List<ProductDto> productsDto = _mapper.Map<List<ProductDto>>(products);

            return ServiceResponse<List<ProductDto>>.Success(productsDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<ProductDto>> GetByIdAsync(string id)
        {
            Product product = await (await _productCollection.FindAsync(product => product.Id == id)).FirstOrDefaultAsync();

            if (product == null)
                throw new NotFoundException("Product not found");

            ProductDto productDto = _mapper.Map<ProductDto>(product);

            return ServiceResponse<ProductDto>.Success(productDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<List<ProductDto>>> GetAllByUserAsync(string userId)
        {
            List<Product> products = await (await _productCollection.FindAsync(p => p.UserId == userId)).ToListAsync();

            List<ProductDto> productsDto = _mapper.Map<List<ProductDto>>(products);

            return ServiceResponse<List<ProductDto>>.Success(productsDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<NoContent>> CreateAsync(CreateProductDto model)
        {
            if (model == null)
                throw new BadRequestException("Create product model is null or empty");

            Product product = _mapper.Map<Product>(model);

            await _productCollection.InsertOneAsync(product);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status201Created);
        }
        public async Task<ServiceResponse<NoContent>> UpdateAsync(UpdateProductDto model)
        {
            if (model == null)
                throw new BadRequestException("Update Product Model is null or empty");

            Product course = await (await _productCollection.FindAsync(course => course.Id == model.Id)).FirstOrDefaultAsync();

            if (course == null)
                throw new NotFoundException("Product not found");

            Product updatedCourse = _mapper.Map(model, course);
            Task updateTask = _productCollection.FindOneAndReplaceAsync(c => c.Id == model.Id, updatedCourse);

            //inbox outbox
            var idempotentToken = Guid.NewGuid();

            ProductUpdatedEvent productUpdatedEvent = new()
            {
                IdempotentToken = idempotentToken,
                PictureUrl = model.ImagePath,
                Price = model.Price,
                ProductId = model.Id,
                ProductName = model.Name
            };
            CatalogOutbox catalogOutbox = new()
            {
                IdempotentToken = idempotentToken,
                OccuredOn = DateTime.UtcNow,
                ProcessedOn = null,
                Payload = JsonSerializer.Serialize(productUpdatedEvent),
                Type = productUpdatedEvent.GetType().Name
            };

            await _dbContext.CatalogOutboxes.AddAsync(catalogOutbox);
            Task saveOutboxTask = _dbContext.SaveChangesAsync();
            await Task.WhenAll(updateTask, saveOutboxTask);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }
        public async Task<ServiceResponse<NoContent>> DeleteAsync(string id)
        {
            if (id == string.Empty)
                throw new BadRequestException("Id is null or empty");

            bool isExist = await (await _productCollection.FindAsync(course => course.Id == id)).AnyAsync();

            if (!isExist)
                throw new NotFoundException("Product not found");

            Task deleteTask = _productCollection.FindOneAndDeleteAsync(c => c.Id == id);

            //inbox outbox
            var idempotentToken = Guid.NewGuid();
            ProductDeletedEvent productDeletedEvent = new()
            {
                IdempotentToken = idempotentToken,
                ProductId = id
            };
            CatalogOutbox catalogOutbox = new()
            {
                IdempotentToken = idempotentToken,
                OccuredOn = DateTime.UtcNow,
                ProcessedOn = null,
                Payload = JsonSerializer.Serialize(productDeletedEvent),
                Type = productDeletedEvent.GetType().Name
            };

            await _dbContext.CatalogOutboxes.AddAsync(catalogOutbox);
            Task saveOutboxTask = _dbContext.SaveChangesAsync();

            await Task.WhenAll(saveOutboxTask, deleteTask);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }

    }
}
