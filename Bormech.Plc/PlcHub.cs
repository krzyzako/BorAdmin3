using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Bormech.Plc;

public class PlcHub : Hub
{
    private readonly PlcService _plcService;
    private readonly ILogger<PlcHub> _logger;
    private bool _disposed = false;

    public PlcHub(PlcService plcService, ILogger<PlcHub> logger)
    {
        _plcService = plcService;
        _logger = logger;

        // Subscribe to the event
        _plcService.DataChanged += async (plcData) => await SendMessage(plcData);
    }

    // Event handler to broadcast the event to connected clients
    private async Task SendMessage(OutGoPlc plcSchedule)
    {
        _logger.LogInformation($"PlcDataChanged event received 1-> {plcSchedule.Wanna1Temperature}");
        if (_disposed)
        {
            _logger.LogWarning("PlcHub is disposed");
            return;
        }
        var plcData = plcSchedule;
        _logger.LogInformation($"PlcScheduleChanged event received -> {plcSchedule.Wanna1Temperature}");
        await Clients.All.SendAsync("PlcScheduleChanged", plcData);
    }

    // This method can be called by clients to check the connection
    public async Task CheckConnection()
    {
        if (_disposed)
        {
            _logger.LogWarning("PlcHub is disposed");
            return;
        }
        await Clients.Caller.SendAsync("ConnectionChecked", "Connection is active");
    }
    public async Task<PlcSchedule> SendObjectToHub(string obj)
    {
        // Logika przetwarzania obiektu
        Console.WriteLine($"Received object: ");
        var schelud = await _plcService.ReadScheduleTaskAsync();
        
        return schelud;
    }

    public async Task SaveSchedule(PlcSchedule schedule)
    {
        await _plcService.WriteSchedule(schedule);
        await Clients.All.SendAsync("SendAfterSaveSchedule", schedule);
        Console.WriteLine("dddddddddddddddddddd");
    }
    public async Task<TimeSpan> GetTimeScheluder(ReadWordObject obj)
    {
        var hour  = await _plcService.ReadWordAsync(obj.Db,obj.Start);
        var minute = await _plcService.ReadWordAsync(obj.Db,obj.Start+4);
        var time = new TimeSpan(hour,minute,0); 
        Console.WriteLine(time);
        return time; 
    }
    public async Task SaveTimeScheluder(SavedTime obj)
    {
        Console.WriteLine(obj.Czas.Hours);
        Console.WriteLine(obj.Czas.Minutes);
        Console.WriteLine(obj.ReadWordObject.Start);
        var hour  = await _plcService.WriteWordAsync(obj.ReadWordObject.Db,obj.ReadWordObject.Start,obj.Czas.Hours);
        var minute = await _plcService.WriteWordAsync(obj.ReadWordObject.Db,obj.ReadWordObject.Start+4,obj.Czas.Minutes); 
        
        // Console.WriteLine(hour+":"+minute);
         
    }
    protected override void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Unsubscribe from the event
            _plcService.DataChanged -= async (plcData) => await SendMessage(plcData);
        }

        _disposed = true;
        base.Dispose(disposing);
    }
}