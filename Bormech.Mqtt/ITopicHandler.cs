namespace Bormech.Mqtt;

public interface ITopicHandler
{
    void Handle(string payload);
}