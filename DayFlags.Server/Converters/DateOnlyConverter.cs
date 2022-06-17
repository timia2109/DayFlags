using Newtonsoft.Json;

namespace DayFlags.Server.Converters;

public class DateOnlyConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
        => objectType == typeof(DateOnly) || objectType == typeof(TimeOnly)
        || objectType == typeof(DateOnly?) || objectType == typeof(TimeOnly?);

    public override bool CanRead => false;

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            serializer.Serialize(writer, null);
        }

        if (value is DateOnly d)
        {
            serializer.Serialize(writer, d.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd"));
        }
        else if (value is TimeOnly t)
        {
            serializer.Serialize(writer, t.ToTimeSpan().ToString());
        }
    }
}