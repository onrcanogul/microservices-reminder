using AutoMapper;
using Microservices.CatalogAPI.Configurations;
using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Models;
using Microservices.CatalogAPI.Services.Abstractions;
using Microservices.Shared.Dtos;
using Microservices.Shared.Exceptions;
using MongoDB.Driver;

namespace Microservices.CatalogAPI.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            MongoClient client = new MongoClient(databaseSettings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<CategoryDto>>> GetAllAsync()
        {
            List<Category> categories = await (await _categoryCollection.FindAsync(category => true)).ToListAsync();

            List<CategoryDto> categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

            return ServiceResponse<List<CategoryDto>>.Success(categoriesDto, StatusCodes.Status200OK);
        }

        public async Task<ServiceResponse<CategoryDto>> GetByIdAsync(string id)
        {
            Category category = await (await _categoryCollection.FindAsync(c => c.Id == id)).FirstOrDefaultAsync();

            if (category == null)
                throw new NotFoundException("Category not found");

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(category);

            return ServiceResponse<CategoryDto>.Success(categoryDto, StatusCodes.Status200OK);
        }

        public async Task<ServiceResponse<NoContent>> CreateAsync(CreateCategoryDto model)
        {
            Category category = _mapper.Map<Category>(model);

            await _categoryCollection.InsertOneAsync(category);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status201Created);
        }
        public async Task<ServiceResponse<NoContent>> UpdateAsync(UpdateCategoryDto model)
        {
            Category category = await (await _categoryCollection.FindAsync(category => category.Id == model.Id)).FirstOrDefaultAsync();

            if (category == null)
                throw new NotFoundException("Category not found");

            category.Name = model.Name;

            await _categoryCollection.FindOneAndReplaceAsync(c => c.Id == category.Id, category);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }
        public async Task<ServiceResponse<NoContent>> DeleteAsync(string id)
        {
            bool isExist = await (await _categoryCollection.FindAsync(category => category.Id == id)).AnyAsync();

            if (!isExist)
                throw new NotFoundException("Category not found");

            await _categoryCollection.FindOneAndDeleteAsync(c => c.Id == id);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status204NoContent);
        }
    }
}
