using Bormech.Mqtt.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MQTTnet;

namespace Bormech.Mqtt;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMqttServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MqttService>();
        services.AddHostedService<MqttHost>();
        services.AddTransient<TemperatureHandler>();
        services.AddTransient<HumidityHandler>();
        
        services.AddSingleton<HandlerFactory>(sp =>
        {
            var handlerFactory = new HandlerFactory();
            handlerFactory.RegisterHandler("bell/send/+/temperature", (sp.GetRequiredService<TemperatureHandler>)); 
            handlerFactory.RegisterHandler("bell/send/+/humidity",(sp.GetRequiredService<HumidityHandler>));
            handlerFactory.RegisterHandler("bell/send/+/pression",(sp.GetRequiredService<TemperatureHandler>));
            return handlerFactory;
        });
        
        services.AddHostedService<MqttRouter>();
        return services;
        
    }   
}