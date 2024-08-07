using MassTransit;
using Microservices.CatalogAPI.Configurations;
using Microservices.CatalogAPI.Contexts;
using Microservices.CatalogAPI.Models;
using Microservices.Shared.Events;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;


namespace Microservices.CatalogAPI.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedInboxToConsumerEvent>
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly OrderInboxDbContext _orderInboxContext;
        public OrderCreatedEventConsumer(IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            MongoClient mongoClient = new(databaseSettings.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(databaseSettings.CourseCollectionName);
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<OrderCreatedInboxToConsumerEvent> context)
        {
            List<bool> results = new();

            try
            {
                foreach (var orderItem in context.Message.@event.OrderItems)
                    results.Add(await _productCollection.Find(c => c.Id == orderItem.ProductId).AnyAsync());
            }
            catch (Exception ex)
            {
                ProductNotFoundEvent productNotFoundEvent = new()
                {
                    OrderId = context.Message.@event.OrderId,
                    Message = "product not found"
                };
                await _publishEndpoint.Publish(productNotFoundEvent);
            }

            if (results.TrueForAll(r => r.Equals(true)))
            {
                ProductFoundEvent productFoundEvent = new()
                {
                    BuyerId = context.Message.@event.BuyerId,
                    OrderId = context.Message.@event.OrderId,
                    TotalPrice = context.Message.@event.TotalPrice
                };
                await _publishEndpoint.Publish(productFoundEvent);
            }
            else
            {
                ProductNotFoundEvent productNotFoundEvent = new()
                {
                    OrderId = context.Message.@event.OrderId,
                    Message = "product not found"
                };
                await _publishEndpoint.Publish(productNotFoundEvent);
            }
        }






    }
}

