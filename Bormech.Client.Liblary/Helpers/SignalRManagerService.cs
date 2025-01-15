using Microsoft.AspNetCore.SignalR.Client;

namespace Bormech.Client.Liblary.Helpers;

public class SignalRManagerService
{
    private readonly Dictionary<string, HubConnection> _hubConnections = new();

    public void AddHub(string hubName, Uri hubUrl)
    {
        if (!_hubConnections.ContainsKey(hubName))
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            _hubConnections[hubName] = connection;
        }
    }

    public async Task StartHubAsync(string hubName)
    {
        if (_hubConnections.TryGetValue(hubName, out var connection) && connection.State == HubConnectionState.Disconnected)
        {
            await connection.StartAsync();
        }
    }

    public async Task StopHubAsync(string hubName)
    {
        if (_hubConnections.TryGetValue(hubName, out var connection) && connection.State == HubConnectionState.Connected)
        {
            await connection.StopAsync();
        }
    }
    public async Task<T> InvokeAsync<T>(string hubName, string methodName, object arg)
    {
        if (_hubConnections.TryGetValue(hubName, out var connection))
        {
            Console.WriteLine("InvokeAsync -> " + methodName +" arg -> " + arg);
            return await connection.InvokeAsync<T>(methodName, arg);
        }
        throw new Exception($"Hub {hubName} is not registered or not connected.");
    }
    public async Task<T> InvokeAsync<T>(string hubName, string methodName)
    {
        if (_hubConnections.TryGetValue(hubName, out var connection))
        {
            Console.WriteLine("InvokeAsync -> " + methodName );
            return await connection.InvokeAsync<T>(methodName);
        }
        throw new Exception($"Hub {hubName} is not registered or not connected.");
    }
    
    public HubConnectionState GetHubConnectionState(string hubName)
    {
        if (_hubConnections.TryGetValue(hubName, out var connection))
        {
            return connection.State;
        }

        throw new KeyNotFoundException($"Hub {hubName} is not registered.");
    }
    public void On<T>(string hubName, string methodName, Action<T> handler)
    {
        if (_hubConnections.TryGetValue(hubName, out var connection))
        {
            connection.On(methodName, handler);
        }
    }
}