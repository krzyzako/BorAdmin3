namespace Bormech.Mqtt;

public class TemperatureHandler(MqttBackgroundService mqtt) : ITopicHandler
{
    public void Handle(string payload)
    {
        Console.WriteLine($"Temperature: {payload}°C");
        _ = mqtt.SendMqttMessageAsync("response", payload);
    }

 
}