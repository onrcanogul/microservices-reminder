using AutoMapper;
using MediatR;
using Microservices.OrderApplication.Dtos;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Dtos;
using Microservices.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderApplication.Feature.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler(OrderDbContext orderDbContext, IMapper mapper) : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            ServiceResponse<OrderDto>? response = null;

            Order? order = await orderDbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == request.Id);

            if (order is null)
                throw new NotFoundException("Order not found");
            else
                response = ServiceResponse<OrderDto>.Success(mapper.Map<OrderDto>(order), 200);

            return new()
            {
                OrderItemResponse = response
            };
        }
    }
}
