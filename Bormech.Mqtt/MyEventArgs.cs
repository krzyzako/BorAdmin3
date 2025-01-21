namespace Bormech.Mqtt;

public class MyEventArgs(string topic, string message): EventArgs
{
    public string Topic { get; set; } = topic;
    public string Message { get; set; } = message;
}