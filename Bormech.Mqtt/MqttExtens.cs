using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;

namespace Bormech.Mqtt;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMqttServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Rejestracja klienta MQTT
        // services.AddHostedService<IMqttClient>(sp =>
        // {
        //     var mqttFactory = new MqttClientFactory();
        //     return mqttFactory.CreateMqttClient();
        // });
        //
        // // Konfiguracja klienta MQTT
        // services.AddSingleton<MqttClientOptions>(sp => new MqttClientOptionsBuilder()
        //     .WithClientId("DotNetMqttClient")
        //     .WithTcpServer("broker.hivemq.com", 1883)
        //     .WithCleanSession()
        //     .Build());
        
        services.AddHostedService<MqttBackgroundService>();

        
        return services;
        
    }   
}

