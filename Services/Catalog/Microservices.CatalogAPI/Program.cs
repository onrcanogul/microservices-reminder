
using MassTransit;
using Microservices.CatalogAPI.Configurations;
using Microservices.CatalogAPI.Consumers;
using Microservices.CatalogAPI.Contexts;
using Microservices.CatalogAPI.Services.Abstractions;
using Microservices.CatalogAPI.Services.Concretes;
using Microservices.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();



builder.Services.AddDbContext<CatalogOutboxDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));

});

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderCreatedEventConsumer>();
    config.UsingRabbitMq((context, configure) =>
    {
        configure.Host(builder.Configuration["RabbitMQ"]);
        configure.ReceiveEndpoint(RabbitMqSettings.OrderInbox_OrderCreatedInboxEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
    });
});

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});


builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
