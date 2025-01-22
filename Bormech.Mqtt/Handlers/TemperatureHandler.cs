using System.Text.Json;
using Bormech.Server.Liblary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bormech.Mqtt.Handlers;

public class TemperatureHandler(MqttService mqtt, ILogger<TemperatureHandler> logger, IServiceScopeFactory serviceScopeFactory) : ITopicHandler
{
    private IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory; 
    private MqttService _mqtt = mqtt;
    private ILogger<TemperatureHandler> _logger = logger;
    private Dictionary<string, object> _userDict = new();
    private int _id = 0;
    public async Task Handle(string device,string payload)
    {
      
        _logger.LogInformation("TemperatureHandler.Handle");
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var user = await dbContext.ApplicationUsers.FirstAsync(x => x.Id == 1);
        _id++;
        if (user.FullName != null && user.Email != null )
        {
            var userDict = new Dictionary<string, object>
            {
                { "name", user.FullName },
                { "email", user.Email },
                { "id", _id }
            };
            if (float.TryParse(payload, out var result)) userDict.Add("temperature", result);
            var json = JsonSerializer.Serialize(userDict);
            await _mqtt.SendMqttMessageAsync("bell/recive/"+device+"/user", json);
            
        }

        // if (_mqtt is not null)
        // {
        //    await _mqtt.SendMqttMessageAsync("bell/recive/"+device+"/temperature", json);
        // }
    }
}