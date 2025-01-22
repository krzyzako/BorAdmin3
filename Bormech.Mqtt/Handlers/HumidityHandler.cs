using Microsoft.Extensions.Logging;

namespace Bormech.Mqtt.Handlers;

public class HumidityHandler(ILogger<HumidityHandler> logger):ITopicHandler
{
    public Task Handle(string device, string payload)
    {
        logger.LogInformation("HumidityHandler.Handle");
        return Task.CompletedTask;
    }
}