using AutoMapper;
using Dapper;
using Microservices.DiscountAPI.Dtos;
using Microservices.DiscountAPI.Models;
using Microservices.DiscountAPI.Services.Abstractions;
using Microservices.Shared.Dtos;
using Npgsql;
using System.Data;

namespace Microservices.DiscountAPI.Services.Concretes
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;
        private readonly IMapper _mapper;

        public DiscountService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSQL"));
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<DiscountDto>>> GetDiscountsAsync()
        {
            IEnumerable<Discount> discounts = await _dbConnection.QueryAsync<Discount>("SELECT * FROM discount");
            List<DiscountDto> discountsDto = _mapper.Map<List<DiscountDto>>(discounts);

            return ServiceResponse<List<DiscountDto>>.Success(discountsDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<DiscountDto>> GetDiscountByIdAsync(int id)
        {
            Discount? discount = (await _dbConnection.QueryAsync<Discount>("SELECT * FROM discount WHERE id=@Id", new { Id = id })).SingleOrDefault();
            if (discount == null)
                return ServiceResponse<DiscountDto>.Failure("Discount not found", StatusCodes.Status404NotFound);

            DiscountDto discountDto = _mapper.Map<DiscountDto>(discount);

            return ServiceResponse<DiscountDto>.Success(discountDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<List<DiscountDto>>> GetUsersDiscountAsync(string userId)
        {
            IEnumerable<Discount> discounts = await _dbConnection.QueryAsync<Discount>("SELECT * FROM discount WHERE userId=@Id", new { Id = userId });
          
            List<DiscountDto> discountsDto = _mapper.Map<List<DiscountDto>>(discounts);

            return ServiceResponse<List<DiscountDto>>.Success(discountsDto, StatusCodes.Status200OK);
        }
        public async Task<ServiceResponse<DiscountDto>> GetConfirmedCodeAsync(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Discount>("SELECT * FROM discount WHERE userid=@UserId AND code=@Code", new { UserId = userId, Code = code });

            var hasDiscount = discounts.FirstOrDefault();

            if (hasDiscount == null)
                ServiceResponse<DiscountDto>.Failure("Discount not found", 404);

            DiscountDto discountDto = _mapper.Map<DiscountDto>(hasDiscount);

            return ServiceResponse<DiscountDto>.Success(discountDto, 200);
        }
        public async Task<ServiceResponse<NoContent>> CreateDiscountAsync(CreateDiscountDto createDiscountDto)
        {
            if (createDiscountDto == null)
                return ServiceResponse<NoContent>.Failure("Discount is null or empty", 400);



            var status = await _dbConnection.ExecuteAsync("INSERT INTO discount (userId, rate, code) VALUES(@UserId, @Rate, @Code)", createDiscountDto);
            if (status > 0)
                return ServiceResponse<NoContent>.Success(204);

            return ServiceResponse<NoContent>.Failure("an error accured while adding", 500);
        }
        public async Task<ServiceResponse<NoContent>> UpdateDiscountAsync(UpdateDiscountDto updateDiscountDto)
        {
            if (updateDiscountDto == null)
                return ServiceResponse<NoContent>.Failure("Discount is null or empty", 400);

            var status = await _dbConnection.ExecuteAsync("UPDATE discount SET userId=@UserId, code=@Code, rate=@Rate where id= @id", new { id = updateDiscountDto.Id, UserId = updateDiscountDto.UserId, Code = updateDiscountDto.Code, Rate = updateDiscountDto.Rate });

            if (status > 0)
                return ServiceResponse<NoContent>.Success(204);
            return ServiceResponse<NoContent>.Failure("Discount is not found", 404);
        }
        public async Task<ServiceResponse<NoContent>> DeleteDiscountAsync(int id)
        {
            int status = await _dbConnection.ExecuteAsync("DELETE FROM discount WHERE id=@Id", new { Id = id });

            if (status > 0)
                return ServiceResponse<NoContent>.Success(204);
            return ServiceResponse<NoContent>.Failure("Discount not found", 404);
        }
        
    }
}
