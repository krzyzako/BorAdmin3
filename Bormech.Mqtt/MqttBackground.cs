using System.Text;
using MQTTnet.Packets;
using MQTTnet;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Bormech.Mqtt;

public class MqttBackgroundService
    : BackgroundService
{
    private readonly ILogger<MqttBackgroundService> logger; 
    private IMqttClient _mqttClient ;
    private MqttClientOptions _mqttOptions ;
    private readonly HandlerFactory _handlerFactory;
    private readonly MqttRouter _mqttRouter;

    public MqttBackgroundService(ILogger<MqttBackgroundService> logger)
    {
        _handlerFactory = new HandlerFactory();
        _handlerFactory.RegisterHandler("bell/temperature", () => new TemperatureHandler(this));
        _mqttRouter = new MqttRouter(_handlerFactory);
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
            _mqttRouter.RouteMessage(topic, payload);
            logger.LogInformation("Otrzymano wiadomość: Temat: {Topic}, Treść: {Payload}", topic, payload);

            // Logika obsługi wiadomości
            return Task.CompletedTask;
        };
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Uruchamianie serwisu MQTT.");

            if (!_mqttClient.IsConnected)
            {
                try
                {
                    await _mqttClient.ConnectAsync(_mqttOptions, stoppingToken);
                    logger.LogInformation("Połączono z brokerem MQTT.");
                }
                catch (Exception ex)
                {
                    logger.LogError("Błąd podczas łączenia z brokerem MQTT: {Message}", ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Odczekaj przed ponowną próbą
                }
            }

        
    }

    // public override async Task StartAsync(CancellationToken cancellationToken)
    // {
    //     logger.LogInformation("Uruchamianie serwisu MQTT.");
    //
    //     while (!cancellationToken.IsCancellationRequested)
    //     {
    //         if (!_mqttClient.IsConnected)
    //         {
    //             try
    //             {
    //                 await _mqttClient.ConnectAsync(_mqttOptions, cancellationToken);
    //             }
    //             catch (Exception ex)
    //             {
    //                 logger.LogError("Błąd podczas łączenia z brokerem MQTT: {Message}", ex.Message);
    //                 await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken); // Odczekaj przed ponowną próbą
    //             }
    //         }
    //
    //         await Task.Delay(1000, cancellationToken); // Regularne sprawdzanie
    //     }
    // }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Zatrzymywanie serwisu MQTT.");

        if (_mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync(cancellationToken: cancellationToken);
        }

    }
    public async Task SendMqttMessageAsync(string topic, string payload)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("bell/" + topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
            .Build();
        await _mqttClient.PublishAsync(message);
    }
}



