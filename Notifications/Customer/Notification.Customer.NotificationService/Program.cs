namespace Notification.Customer.NotificationService
{
    using MassTransit;
    using Messaging.Core.Commands;
    using Messaging.Core.Constants;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Notification.Customer.EmailService.Models.Configurations;
    using Notification.Customer.EmailService.Senders;
    using Notification.Customer.EmailService.Senders.Impl;
    using Notification.Customer.NotificationService.Core.Consumers;
    using RabbitMQ.Client;
    using System.Net.Mime;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;
                    var emailConfig = hostContext.Configuration
                    .GetSection("EmailConfiguration")
                    .Get<EmailConfig>();

                    services.AddSingleton(emailConfig);
                    services.AddScoped<IEmailSender, EmailSender>();

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

                        busRegistrationConfigurator.AddConsumer<SendOrderToUserEmailConsumer, SendOrderToUserEmailConsumerDefinition>();
                    });
                });

            return hostBuilder;
        }
    }
}