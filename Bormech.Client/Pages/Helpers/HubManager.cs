using Bormech.Client.Liblary.Helpers;
using Bormech.Plc;
using Microsoft.AspNetCore.Components;

namespace Bormech.Client.Pages.Helpers;


  public class HubManager
{
    private readonly AppConfig _appConfig;
    private readonly SignalRManagerService _signalRManagerService;
    private readonly NavigationManager _navManager;
    private List<Hub> _hubs;
    public HubManager(AppConfig appConfig, SignalRManagerService signalRManagerService, NavigationManager navManager)
    {
        _hubs = new List<Hub>();
        _appConfig = appConfig;
        _signalRManagerService = signalRManagerService;
        _navManager = navManager;
    }

    public async Task<List<Hub>>InitializeHubs(string hubName)
    {
        var hub = _appConfig.FindHubs(hubName);
        foreach (var h in hub)
        {
            // Console.WriteLine(h.Name);
            if (h.Name != null) _signalRManagerService.AddHub(h.Name, _navManager.ToAbsoluteUri(h.Uri));
            if (h.Name != null) await _signalRManagerService.StartHubAsync(h.Name);
            
        }
        return hub;
    }
    public void On<T>(string hubName, string methodName, Action<T> handler)
    {
        _signalRManagerService.On(hubName, methodName, handler);
    }


}