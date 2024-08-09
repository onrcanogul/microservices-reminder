using MassTransit;
using Microservices.OrderApplication;
using Microservices.OrderApplication.Consumers;
using Microservices.OrderInfrastructure;
using Microservices.Shared;
using Microservices.Shared.Exceptions.Handler;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices();


builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<PaymentCompletedEventConsumer>();
    configure.AddConsumer<PaymentFailedEventConsumer>();
    configure.AddConsumer<ProductNotFoundEventConsumer>();
    configure.AddConsumer<ProductUpdatedEventConsumer>();
    configure.AddConsumer<ProductDeletedEventConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["RabbitMQ"]);
        configurator.ReceiveEndpoint(RabbitMqSettings.Payment_Order_PaymentCompletedEventQueue, e => e.ConfigureConsumer<PaymentCompletedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.Payment_Order_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.Catalog_ProductNotAvailableMessageQueue, e => e.ConfigureConsumer<ProductNotFoundEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.CatalogInbox_ProductUpdatedEventQueue, e => e.ConfigureConsumer<ProductUpdatedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMqSettings.CatalogInbox_ProductDeletedEventQueue, e => e.ConfigureConsumer<ProductDeletedEventConsumer>(context));
    });
});


builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});
builder.Services.AddExceptionHandler<ExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(options => { });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
