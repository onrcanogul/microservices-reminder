using MediatR;
using Microservices.OrderApplication.Feature.Commands.CreateOrder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Microservices.OrderApplication
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);
        }


    }
}
