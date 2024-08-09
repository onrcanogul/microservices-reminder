using FluentValidation;
using Microservices.DiscountAPI.Dtos.Validators;
using Microservices.DiscountAPI.Services.Abstractions;
using Microservices.DiscountAPI.Services.Concretes;
using Microservices.Shared.Exceptions.Handler;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateDiscountValidator>();

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
