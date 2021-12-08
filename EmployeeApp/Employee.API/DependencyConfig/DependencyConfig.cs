using Employee.API.EventConsumer;
using Employee.Interfaces;
using Employee.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System;
using System.Linq;

namespace Employee.API.DependencyConfig
{
    public static class DependencyConfig
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IBusService, BusService>();
        }

        public static void AddServiceBus(this IServiceCollection services,
                                         IConfiguration configurations)
        {
            var massTransitConfig = configurations.GetSection("MassTransit");
            var _protocol = massTransitConfig["Protocol"];
            var _rabbitMQHost = massTransitConfig["ClusterUrl"];
            var _rabbitMQUserName = massTransitConfig["UserName"];
            var _rabbitMQPassword = massTransitConfig["Password"];
            var nodes = massTransitConfig.GetSection("Nodes").Get<string[]>();
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri($"{_protocol}://{_rabbitMQHost}"),
                                        hostConfig =>
                                        {
                                            hostConfig.Username(_rabbitMQUserName);
                                            hostConfig.Password(_rabbitMQPassword);
                                            if (nodes != null && nodes.Any())
                                                hostConfig.UseCluster(e =>
                                                {
                                                    foreach (var node in nodes)
                                                    {
                                                        e.Node(node);
                                                    }
                                                });
                                        });

                    services.AddSingleton(c => host);

                    cfg.ReceiveEndpoint(host, "GG.EmployeeCreatedEvent.SVC", ep =>
                    {
                        ep.BindMessageExchanges = false;
                        ep.Bind("Employee:EmployeeTransaction", bconfig =>
                        {
                            bconfig.ExchangeType = ExchangeType.Topic;
                            bconfig.Durable = true;
                            bconfig.RoutingKey = "GG.Employee.Create";
                        });
                        ep.ConfigureConsumer<EmployeeCreatedEvent>(provider);
                    });

                    cfg.ReceiveEndpoint(host, "GG.EmployeeUpdatedEvent.SVC", ep =>
                    {
                        ep.BindMessageExchanges = false;
                        ep.Bind("Employee:EmployeeTransaction", bconfig =>
                        {
                            bconfig.ExchangeType = ExchangeType.Topic;
                            bconfig.Durable = true;
                            bconfig.RoutingKey = "GG.Employee.Update";
                        });
                        ep.ConfigureConsumer<EmployeeCreatedEvent>(provider);
                    });

                    cfg.ReceiveEndpoint(host, "GG.EmployeeDeletedEvent.SVC", ep =>
                    {
                        ep.BindMessageExchanges = false;
                        ep.Bind("Employee:EmployeeTransaction", bconfig =>
                        {
                            bconfig.ExchangeType = ExchangeType.Topic;
                            bconfig.Durable = true;
                            bconfig.RoutingKey = "GG.Employee.Delete";
                        });
                        ep.ConfigureConsumer<EmployeeCreatedEvent>(provider);
                    });

                    cfg.Publish<EmployeeTransaction>(publishConfig => publishConfig.ExchangeType = ExchangeType.Topic);
                    cfg.Send<EmployeeTransaction>(sendConfig => sendConfig.UseRoutingKeyFormatter(y => $"GG.Employee.{y.Message.Notification.TransactionType}"));
                }));
            });

            // Start Service Bus
            var busControl = services.BuildServiceProvider()
                                     .GetService<IBusControl>();

            busControl?.Start();
        }
    }
}
