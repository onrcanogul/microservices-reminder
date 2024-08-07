using MassTransit;
using Microservices.OrderInboxConsumerService;
using Microservices.OrderInboxTableConsumerService.Consumers;
using Microservices.Shared;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddDbContext<OrderInboxDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});


builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<OrderCreatedEventInboxConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["RabbitMQ"]);
        configurator.ReceiveEndpoint(RabbitMqSettings.OrderOutbox_OrderCreatedInboxEventQueue, e => e.ConfigureConsumer<OrderCreatedEventInboxConsumer>(context));
    });
});
var host = builder.Build();
host.Run();
