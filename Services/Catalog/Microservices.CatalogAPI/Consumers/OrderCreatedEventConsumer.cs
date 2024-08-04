using AutoMapper;
using MassTransit;
using Microservices.CatalogAPI.Configurations;
using Microservices.CatalogAPI.Models;
using Microservices.Shared.Events;
using MongoDB.Driver;

namespace Microservices.CatalogAPI.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderCreatedEventConsumer(IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            MongoClient mongoClient = new(databaseSettings.ConnectionString);
            IMongoDatabase database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> results = new();

            try
            {
                foreach (var orderItem in context.Message.OrderItems)
                    results.Add(await _courseCollection.Find(c => c.Id == orderItem.ProductId).AnyAsync());
            }
            catch (Exception ex)
            {
                ProductNotFoundEvent productNotFoundEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "product not found"
                };
                await _publishEndpoint.Publish(productNotFoundEvent);
            }
            
            if(results.TrueForAll(r => r.Equals(true)))
            {
                ProductFoundEvent productFoundEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice
                };
                await _publishEndpoint.Publish(productFoundEvent);
            }
            else
            {
                ProductNotFoundEvent productNotFoundEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "product not found"
                };
                await _publishEndpoint.Publish(productNotFoundEvent);
            }
        }
    }
}
