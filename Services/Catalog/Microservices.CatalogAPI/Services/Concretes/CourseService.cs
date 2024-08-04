using AutoMapper;
using MassTransit;
using Microservices.CatalogAPI.Configurations;
using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Models;
using Microservices.CatalogAPI.Services.Abstractions;
using Microservices.Shared.Dtos;
using Microservices.Shared.Events;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using MongoDB.Driver;

namespace Microservices.CatalogAPI.Services.Concretes
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            MongoClient mongoClient = new(databaseSettings.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);

            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<ServiceResponse<List<CourseDto>>> GetAllAsync()
        {

            List<Course> courses = await (await _courseCollection.FindAsync(course => true)).ToListAsync();

            List<CourseDto> coursesDto = _mapper.Map<List<CourseDto>>(courses);

            return ServiceResponse<List<CourseDto>>.Success(coursesDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<CourseDto>> GetByIdAsync(string id)
        {
            Course course = await (await _courseCollection.FindAsync(course => course.Id == id)).FirstOrDefaultAsync();

            if (course == null)
                return ServiceResponse<CourseDto>.Failure("Course Not Found", StatusCodes.Status404NotFound);

            CourseDto courseDto = _mapper.Map<CourseDto>(course);

            return ServiceResponse<CourseDto>.Success(courseDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<List<CourseDto>>> GetAllByUserAsync(string userId)
        {
            List<Course> courses = await (await _courseCollection.FindAsync(c => c.UserId == userId)).ToListAsync();

            List<CourseDto> coursesDto = _mapper.Map<List<CourseDto>>(courses);

            return ServiceResponse<List<CourseDto>>.Success(coursesDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<NoContent>> CreateAsync(CreateCourseDto model)
        {
            if (model == null)
                return ServiceResponse<NoContent>.Failure("Create Course Model Not Found", StatusCodes.Status400BadRequest);

            Course course = _mapper.Map<Course>(model);

            await _courseCollection.InsertOneAsync(course);

            return ServiceResponse<NoContent>.Success(StatusCodes.Status201Created);
        }
        public async Task<ServiceResponse<NoContent>> UpdateAsync(UpdateCourseDto model)
        {
            if (model == null)
                return ServiceResponse<NoContent>.Failure("Update Course Model Not Found", StatusCodes.Status400BadRequest);

            Course course = await (await _courseCollection.FindAsync(course => course.Id == model.Id)).FirstOrDefaultAsync();
            if (course == null)
                return ServiceResponse<NoContent>.Failure("Course Not Found", StatusCodes.Status404NotFound);

            Course updatedCourse = _mapper.Map(model, course);

            await _courseCollection.FindOneAndReplaceAsync(c => c.Id == model.Id, updatedCourse);

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

            bool isExist = await (await _courseCollection.FindAsync(course => course.Id == id)).AnyAsync();

            if (!isExist)
                return ServiceResponse<NoContent>.Failure("Course Not Found", StatusCodes.Status404NotFound);

            await _courseCollection.FindOneAndDeleteAsync(c => c.Id == id);

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
