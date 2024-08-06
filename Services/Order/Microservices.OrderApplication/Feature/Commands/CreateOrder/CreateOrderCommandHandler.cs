using AutoMapper;
using MassTransit;
using MediatR;
using Microservices.OrderApplication.Dtos;
using Microservices.OrderDomain.OrderAggregates;
using Microservices.OrderDomain.Outboxes;
using Microservices.OrderInfrastructure;
using Microservices.Shared.Dtos;
using Microservices.Shared.Events;
using System.Text.Json;

namespace Microservices.OrderApplication.Feature.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(OrderDbContext orderDbContext, IMapper mapper) : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            Address address = mapper.Map<Address>(request.Address);

            Order order = new(request.BuyerId, address);
            order.TotalPrice = order.GetTotalPrice;

            foreach (OrderItemDto orderItem in request.OrderItems)
                order.AddOrderItem(orderItem.ProductId, orderItem.PictureUrl, orderItem.Price, orderItem.ProductName);

            await orderDbContext.Orders.AddAsync(order);

            var idempotentToken = Guid.NewGuid();
            OrderCreatedEvent orderCreatedEvent = new()
            {
                IdempotentToken = idempotentToken,
                BuyerId = request.BuyerId,
                OrderId = order.Id,
                TotalPrice = order.TotalPrice,
                OrderItems = order.OrderItems.Select(o => new Shared.Messages.OrderItemMessage
                {
                    Count = o.Count,
                    PictureUrl = o.PictureUrl,
                    Price = o.Price,
                    ProductId = o.ProductId,
                    ProductName = o.ProductName
                }).ToList()
            };
            OrderOutbox orderOutbox = new()
            {
                IdempotentToken = idempotentToken,
                OccuredOn = DateTime.UtcNow,
                ProcessedOn = null,
                Type = orderCreatedEvent.GetType().Name,
                Payload = JsonSerializer.Serialize(orderCreatedEvent)
            };

            await orderDbContext.OrderOutboxes.AddAsync(orderOutbox);

            await orderDbContext.SaveChangesAsync(); // save changes at the same time.

            return new(ServiceResponse<NoContent>.Success(204));
        }
    }
}
