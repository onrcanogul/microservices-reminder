using AutoMapper;
using MediatR;
using Microservices.OrderApplication.Dtos;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.OrderApplication.Feature.Queries.GetOrderByUserId
{
    public class GetOrdersByUserIdQueryHandler(OrderDbContext orderDbContext, IMapper mapper) : IRequestHandler<GetOrdersByUserIdQueryRequest, GetOrdersByUserIdQueryResponse>
    {
        public async Task<GetOrdersByUserIdQueryResponse> Handle(GetOrdersByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            List<Order> orders = await orderDbContext
                .Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.BuyerId == request.UserId)
                .ToListAsync();

            List<OrderDto> ordersDto = mapper.Map<List<OrderDto>>(orders);

            return new()
            {
                Orders = ServiceResponse<List<OrderDto>>.Success(ordersDto, 200)
            };
        }
    }
}
