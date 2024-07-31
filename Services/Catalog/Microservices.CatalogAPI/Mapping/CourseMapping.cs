using AutoMapper;
using Microservices.CatalogAPI.Dtos;
using Microservices.CatalogAPI.Models;

namespace Microservices.CatalogAPI.Mapping
{
    public class CourseMapping : Profile
    {
        public CourseMapping() 
        {
            CreateMap<CourseDto, Course>().ReverseMap();
            CreateMap<CreateCourseDto, Course>().ReverseMap();
            CreateMap<UpdateCourseDto, Course>().ReverseMap();
        }
    }
}
