using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        
        services.AddSingleton<MqttService>();
        services.AddHostedService<MqttHost>();
        services.AddSingleton<TemperatureHandler>();
        services.AddSingleton<HandlerFactory>(sp =>
        {
            
            var handlerFactory = new HandlerFactory();
            handlerFactory.RegisterHandler("bell/send/#/temperature", (() => sp.GetRequiredService<TemperatureHandler>())); 
            handlerFactory.RegisterHandler("bell/send/#/humidity",(() => sp.GetRequiredService<TemperatureHandler>()));
            handlerFactory.RegisterHandler("bell/send/#/pression",(() => sp.GetRequiredService<TemperatureHandler>()));
            return handlerFactory;
        }
            );
        // services.AddSingleton<MqttRouter>(sp =>
        // {
        //     Console.WriteLine("noooooooooooooooooo");
        //     var mqtt = sp.GetRequiredService<MqttService>();
        //     var handlerFactory = sp.GetRequiredService<HandlerFactory>();
        //     var mqttRouter = new MqttRouter(mqtt,handlerFactory);
        //     return mqttRouter;
        // });
        services.AddHostedService<MqttRouter>();
        
        
        
        return services;
        
    }   
}

