using Microsoft.Extensions.Hosting;

namespace Bormech.Mqtt;

public class MqttHost(MqttService mqttService) : IHostedService
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await mqttService.StartAsync();
    }

    public async  Task StopAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await Task.CompletedTask;
    } 

    public IServiceProvider Services { get; }
}