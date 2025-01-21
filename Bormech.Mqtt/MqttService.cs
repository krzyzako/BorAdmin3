using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Packets;

namespace Bormech.Mqtt;

public sealed class MqttService : BackgroundService
{
    private readonly ILogger<MqttService> logger;
    private IMqttClient _mqttClient;
    private MqttClientOptions _mqttOptions;
    public event Func<MyEventArgs, Task>? MyEvent;

    private async Task OnMyEvent(MyEventArgs message)
    {
        Console.WriteLine("saaaaaaaaaaaaaaa");
        await MyEvent?.Invoke(message)!;
    }
    
    public MqttService(ILogger<MqttService> logger)
    {
        this.logger = logger;
        var mqttFactory = new MqttClientFactory();
        _mqttClient = mqttFactory.CreateMqttClient();
        _mqttOptions = new MqttClientOptionsBuilder()
            .WithClientId("DotNetMqttClient")
            .WithTcpServer("192.168.192.10", 1883)
            .WithCleanSession()
            .Build();

        ConfigureMqttClient();
    }
    
    private void ConfigureMqttClient()
    {
        _mqttClient.ConnectedAsync += async e =>
        {
            logger.LogInformation("Połączono z brokerem MQTT.");

            // Subskrybowanie tematu
            await _mqttClient.SubscribeAsync(new MqttTopicFilter
            {
                Topic = "bell/send/#",
                QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce
            });

            logger.LogInformation("Subskrybowano temat: bell/schedule");
        };

        _mqttClient.DisconnectedAsync += e =>
        {
            logger.LogWarning("Rozłączono z brokerem MQTT. Powód: {Reason}", e.ReasonString);
            return Task.CompletedTask;
        };

        _mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            string topic = e.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            // _mqttRouter.RouteMessage(topic, payload);
            // MyEvent?.Invoke(this,new MyEventArgs(topic,payload));
            var msg = new MyEventArgs(topic, payload);
            _ = OnMyEvent(msg);
            logger.LogInformation("Otrzymano wiadomość: Temat: {Topic}, Treść: {Payload}", topic, payload);

            // Logika obsługi wiadomości
            return Task.CompletedTask;
        };
    }
    public async Task SendMqttMessageAsync(string topic, string payload)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("bell/response/"+ topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
            .Build();
        await _mqttClient.PublishAsync(message);
    }

    public async Task StartAsync()
    {
        await _mqttClient.ConnectAsync(_mqttOptions);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
       _mqttClient.Dispose();
        return Task.CompletedTask;
    }
}
