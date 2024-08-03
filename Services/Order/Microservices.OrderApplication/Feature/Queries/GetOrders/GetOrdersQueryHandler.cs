using AutoMapper;
using MediatR;
using Microservices.OrderApplication.Dtos;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.OrderApplication.Feature.Queries.GetOrders
{
    public class GetOrdersQueryHandler(OrderDbContext orderDbContext, IMapper mapper) : IRequestHandler<GetOrdersQueryRequest, GetOrdersQueryResponse>
    {
        public async Task<GetOrdersQueryResponse> Handle(GetOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            List<Order> orders = await orderDbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .ToListAsync();

            List<OrderDto> ordersDto = mapper.Map<List<OrderDto>>(orders);

            return new()
            {
                OrdersResponse = ServiceResponse<List<OrderDto>>.Success(ordersDto, 200)
            };

        }
    }
}
