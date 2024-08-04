using MassTransit;
using Microservices.Shared;
using Microservices.Shared.Events;

namespace Microservices.PaymentAPI.Consumers
{
    public class ProductFoundEventConsumer(ISendEndpointProvider sendEndpointProvider) : IConsumer<ProductFoundEvent>
    {
        public async Task Consume(ConsumeContext<ProductFoundEvent> context)
        {
            var orderId = context.Message.OrderId;
            var buyerId = context.Message.BuyerId;
            var totalPrice = context.Message.TotalPrice;

            bool result = true;

            if (result)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = orderId
                };

                ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.Payment_Order_PaymentCompletedEventQueue}"));
                await sendEndpoint.Send(paymentCompletedEvent);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = orderId
                };

                ISendEndpoint sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMqSettings.Payment_Order_PaymentFailedEventQueue}"));
                await sendEndpoint.Send(paymentFailedEvent);
            }


        }
    }
}
