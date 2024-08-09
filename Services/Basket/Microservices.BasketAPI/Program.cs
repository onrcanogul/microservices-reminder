using FluentValidation;
using Microservices.BasketAPI.Dtos.Validators;
using Microservices.BasketAPI.Services;
using Microservices.BasketAPI.Services.Abstractions;
using Microservices.BasketAPI.Services.Concretes;
using Microservices.BasketAPI.Settings;
using Microservices.Shared.Exceptions.Handler;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
builder.Services.AddSingleton<RedisService>(sp =>
{
    RedisSettings redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    RedisService redisService = new(redisSettings.Host, redisSettings.Port);
    redisService.Connect();
    return redisService;
});

builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<BasketDtoValidator>();



var app = builder.Build();

// Configure the HTTP request pipeline.
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
