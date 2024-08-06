using MassTransit;
using Microservices.OrderOutboxTablePublisherService;
using Microservices.OrderOutboxTablePublisherService.Jobs;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IOrderOutboxSingletonDatabase, OrderOutboxSingletonDatabase>();

builder.Services.AddQuartz(configure =>
{
    JobKey jobKey = new("OrderOutboxPublishJob");
    configure.AddJob<OrderOutboxPublishJob>(opt => opt.WithIdentity(jobKey));

    TriggerKey triggerKey = new("OrderOutboxPublishTrigger");
    configure.AddTrigger(options => options.ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
    .WithIntervalInSeconds(5)
    .RepeatForever()));
});
builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, configure) =>
    {
        configure.Host(builder.Configuration["RabbitMQ"]);
    });
});

var host = builder.Build();
host.Run();
