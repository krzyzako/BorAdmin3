using System.Text.Json;

namespace Bormech.Client.Liblary.Helpers;

public static class Serializations
{
    public static string SerializeObj<T>(T modelObject)
    {
        return JsonSerializer.Serialize(modelObject);
    }

    public static T? DeserializeObj<T>(string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString);
    }

    public static IList<T>? DeserializeJsonStringList<T>(string jsonString)
    {
        return JsonSerializer.Deserialize<IList<T>>(jsonString);
    }
}