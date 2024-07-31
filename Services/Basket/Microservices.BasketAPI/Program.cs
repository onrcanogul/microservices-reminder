using Microservices.BasketAPI.Services;
using Microservices.BasketAPI.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RedisService>(sp =>
{
    RedisSettings redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
    RedisService redisService = new(redisSettings.Host, redisSettings.Port);
    redisService.Connect();
    return redisService;
});

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
