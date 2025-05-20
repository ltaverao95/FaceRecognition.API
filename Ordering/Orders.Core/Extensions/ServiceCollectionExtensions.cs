namespace Orders.Core.Extensions
{
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Orders.Core.Messages.Consumers;
    using Orders.Core.Models.Configuration;
    using Orders.Core.Persistence;
    using Orders.Core.Persistence.Repository;
    using Orders.Core.Persistence.Repository.Impl;
    using System.Net.Mime;

    public static class ServiceCollectionExtensions
    {
        public static void AddCoreStartup(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddDbContext<OrdersContext>(dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.UseSqlServer(configuration["OrdersContextConnection"], opt =>
                {
                    opt.MigrationsAssembly("Orders.API");
                });
            });

            services.Configure<OrderSettingsConfiguration>(configuration);

            services.AddMassTransit(busRegistrationConfigurator =>
            {
                busRegistrationConfigurator.UsingRabbitMq((context, rabbitBusFactoryConfigurator) =>
                {
                    rabbitBusFactoryConfigurator.DefaultContentType = new ContentType("application/json");
                    rabbitBusFactoryConfigurator.UseRawJsonDeserializer();

                    rabbitBusFactoryConfigurator.Host("rabbitmq", configuration["RabbitConfig:VHost"]!, rabbitHostConfigurator =>
                    {
                        rabbitHostConfigurator.Username(configuration["RabbitConfig:UserName"]!);
                        rabbitHostConfigurator.Password(configuration["RabbitConfig:Password"]!);
                    });

                    rabbitBusFactoryConfigurator.ConfigureEndpoints(context);
                });

                busRegistrationConfigurator.AddConsumer<RegisterOrderConsumer, RegisterOrderConsumerDefinition>();
                busRegistrationConfigurator.AddConsumer<UpdateOrderStatusConsumer, UpdateOrderStatusConsumerDefinition>();
            });

            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}