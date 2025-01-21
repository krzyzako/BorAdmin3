using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bormech.Mqtt;

public class MqttRouter : IHostedService
{
    private readonly HandlerFactory _handlerFactory;
    private readonly ILogger<MqttRouter> _logger;
    public MqttRouter(MqttService mqtt, HandlerFactory handlerFactory, ILogger<MqttRouter> logger)
    {
        _handlerFactory = handlerFactory;
        _logger = logger;
        var dic = _handlerFactory.GetRegisterHandler();
        foreach (var key in dic.Keys)
        {
            Console.WriteLine(key);
        }
        // _ = mqtt.SendMqttMessageAsync("sss", "sss");
        mqtt.MyEvent += async (sender)  => 
        {
            // Console.WriteLine("sad");
            // await mqtt.SendMqttMessageAsync("ssss0", "ssss");
             RouteMessage(sender.Topic, sender.Message);
        };
    }



    // Przekazuje wiadomość do odpowiedniego handlera
    public void RouteMessage(string topic, string payload)
    {
        var handler = _handlerFactory.CreateHandler(topic);
        var device = topic.Split('/')[2];
        Console.WriteLine(device);
        if (handler != null)
            handler.Handle(device,payload);
        else
            Console.WriteLine($"No handler found for topic '{topic}'.");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start");
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Koniec");
        await Task.CompletedTask;
    }
}