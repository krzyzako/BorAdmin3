using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bormech.Data.Converter;


public class EnumDisplayConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var enumValue = reader.GetString();
        Console.WriteLine(reader.GetString());
        if (Enum.TryParse(typeToConvert, enumValue, true, out var result))
        {
            return (T)result;
        }

        throw new JsonException($"Invalid value for enum {typeToConvert.Name}: {enumValue}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var displayName = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .Name ?? value.ToString();

        writer.WriteStringValue(displayName);
    }
    
    
    
}