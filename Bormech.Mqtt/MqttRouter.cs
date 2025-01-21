namespace Bormech.Mqtt;

public class MqttRouter
{
    private readonly HandlerFactory _handlerFactory;

    public MqttRouter(HandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    // Przekazuje wiadomość do odpowiedniego handlera
    public void RouteMessage(string topic, string payload)
    {
        var handler = _handlerFactory.CreateHandler(topic);
        if (handler != null)
            handler.Handle(payload);
        else
            Console.WriteLine($"No handler found for topic '{topic}'.");
    }
}