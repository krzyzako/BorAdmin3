namespace Bormech.Mqtt;

public interface ITopicHandler
{
    Task Handle(string device, string payload);
}