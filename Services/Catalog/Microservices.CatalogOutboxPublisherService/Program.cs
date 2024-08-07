
using MassTransit;
using Microservices.CatalogOutboxPublisherService.Jobs;
using Microservices.CatalogOutboxPublisherService.Services;
using Microservices.OrderOutboxTablePublisherService;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddSingleton<ICatalogOutboxSingletonDatabase, CatalogOutboxSingletonDatabase>();
builder.Services.AddScoped<ICatalogOutboxService, CatalogOutboxService>();

builder.Services.AddQuartz(configure =>
{
    JobKey jobKey = new("CatalogOutboxPublishJob");
    configure.AddJob<CatalogOutboxPublishJob>(opt => opt.WithIdentity(jobKey));

    TriggerKey triggerKey = new("CatalogOutboxPublishTrigger");
    configure.AddTrigger(options => options.ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
    .WithIntervalInSeconds(60)
    .RepeatForever()));
});
builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);


builder.Services.AddMassTransit(configure =>
{
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration["RabbitMQ"]);
    });

});

var host = builder.Build();
host.Run();
