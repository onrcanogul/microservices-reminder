using AutoMapper;
using MediatR;
using Microservices.OrderApplication.Dtos;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.OrderApplication.Feature.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(OrderDbContext orderDbContext, IMapper mapper) : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
			try
			{
                Address address = mapper.Map<Address>(request.Address);

                Order order = new(request.BuyerId, address);
                order.TotalPrice = order.GetTotalPrice;

                foreach (OrderItemDto orderItem in request.OrderItems)
                    order.AddOrderItem(orderItem.ProductId, orderItem.PictureUrl, orderItem.Price, orderItem.ProductName);

                await orderDbContext.Orders.AddAsync(order);
                await orderDbContext.SaveChangesAsync();

                return new(ServiceResponse<NoContent>.Success(204));

            }
			catch (Exception ex)
			{

				throw ex;
			}     
        }
    }
}
