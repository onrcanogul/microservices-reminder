using MassTransit;
using Microservices.CatalogInboxPublisherService.Consumers;
using Microservices.CatalogInboxPublisherService.Contexts;
using Microservices.CatalogInboxPublisherService.Services;
using Microservices.Shared;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddDbContext<CatalogInboxDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});
builder.Services.AddScoped<ICatalogInboxService, CatalogInboxService>();


builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<ProductDeletedEventInboxConsumer>();
    configure.AddConsumer<ProductUpdatedEventInboxConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["RabbitMQ"]);
        configurator.ReceiveEndpoint(RabbitMqSettings.CatalogOutbox_ProductDeletedEventQueue, e => e.ConfigureConsumer<ProductDeletedEventInboxConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.CatalogOutbox_ProductUpdatedEventQueue, e => e.ConfigureConsumer<ProductUpdatedEventInboxConsumer>(context));
    });

});


var host = builder.Build();
host.Run();
