namespace Bormech.Mqtt;

public class HandlerFactory
{
    private readonly Dictionary<string, Func<ITopicHandler>> _handlerCreators = new();
   
    // Rejestracja handlera dla tematu
    public Dictionary<string, Func<ITopicHandler>> GetRegisterHandler()
    {
        return _handlerCreators;
    }
    public void RegisterHandler(string topic, Func<ITopicHandler> handlerCreator)
    {
        Console.WriteLine("Register handler");
        if (_handlerCreators.ContainsKey(topic))
            throw new InvalidOperationException($"Handler for topic '{topic}' is already registered.");

        _handlerCreators[topic] = handlerCreator;
    }

    // Tworzenie handlera na podstawie tematu
    public ITopicHandler? CreateHandler(string topic)
    {
        foreach (var topicPattern in _handlerCreators.Keys)
            if (MqttTopicMatches(topicPattern, topic))
                return _handlerCreators[topicPattern]();

        return null;
    }

    // Sprawdza dopasowanie tematu
    private static bool MqttTopicMatches(string subscribedTopic, string receivedTopic)
    {
        var subscribedParts = subscribedTopic.Split('/');
        var receivedParts = receivedTopic.Split('/');

        for (var i = 0; i < subscribedParts.Length; i++)
        {
            if (i >= receivedParts.Length)
                return false;

            if (subscribedParts[i] == "#")
                return true;

            if (subscribedParts[i] != "+" && subscribedParts[i] != receivedParts[i])
                return false;
        }

        return subscribedParts.Length == receivedParts.Length;
    }
}