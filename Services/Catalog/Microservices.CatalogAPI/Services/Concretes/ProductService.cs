using AutoMapper;
using MassTransit;
using Microservices.CatalogAPI.Configurations;
using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Models;
using Microservices.CatalogAPI.Services.Abstractions;
using Microservices.Shared.Dtos;
using Microservices.Shared.Events;
using MongoDB.Driver;

namespace Microservices.CatalogAPI.Services.Concretes
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            MongoClient mongoClient = new(databaseSettings.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(databaseSettings.DatabaseName);

            _productCollection = database.GetCollection<Product>(databaseSettings.CourseCollectionName);

            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
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
                return ServiceResponse<ProductDto>.Failure("Product Not Found", StatusCodes.Status404NotFound);

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
                return ServiceResponse<NoContent>.Failure("Create Product Model Not Found", StatusCodes.Status400BadRequest);

            Product product = _mapper.Map<Product>(model);

            await _productCollection.InsertOneAsync(product);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status201Created);
        }
        public async Task<ServiceResponse<NoContent>> UpdateAsync(UpdateProductDto model)
        {
            if (model == null)
                return ServiceResponse<NoContent>.Failure("Update Product Model Not Found", StatusCodes.Status400BadRequest);

            Product course = await (await _productCollection.FindAsync(course => course.Id == model.Id)).FirstOrDefaultAsync();
            if (course == null)
                return ServiceResponse<NoContent>.Failure("Product Not Found", StatusCodes.Status404NotFound);

            Product updatedCourse = _mapper.Map(model, course);

            await _productCollection.FindOneAndReplaceAsync(c => c.Id == model.Id, updatedCourse);

            //inbox outbox
            ProductUpdatedEvent productUpdatedEvent = new()
            {
                PictureUrl = model.ImagePath,
                Price = model.Price,
                ProductId = model.Id,
                ProductName = model.Name
            };
            await _publishEndpoint.Publish(productUpdatedEvent);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }
        public async Task<ServiceResponse<NoContent>> DeleteAsync(string id)
        {
            if (id == string.Empty)
                return ServiceResponse<NoContent>.Failure("Id is null", StatusCodes.Status400BadRequest);

            bool isExist = await (await _productCollection.FindAsync(course => course.Id == id)).AnyAsync();

            if (!isExist)
                return ServiceResponse<NoContent>.Failure("Product Not Found", StatusCodes.Status404NotFound);

            await _productCollection.FindOneAndDeleteAsync(c => c.Id == id);

            //inbox outbox
            ProductDeletedEvent productDeletedEvent = new()
            {
                ProductId = id
            };
            await _publishEndpoint.Publish(productDeletedEvent);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }

    }
}
