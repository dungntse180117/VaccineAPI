using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace VaccineAPI.Shared.Helpers;
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private readonly string _format = "yyyy-MM-dd"; // Standard format

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // ✅ Handle cases where dob is an object instead of a string
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                int year = root.GetProperty("year").GetInt32();
                int month = root.GetProperty("month").GetInt32();
                int day = root.GetProperty("day").GetInt32();
                return new DateOnly(year, month, day);
            }
        }
        // ✅ Handle standard string format "yyyy-MM-dd"
        else if (reader.TokenType == JsonTokenType.String)
        {
            string dateString = reader.GetString();
            return DateOnly.ParseExact(dateString, _format);
        }
        else
        {
            throw new JsonException($"Unexpected JSON token {reader.TokenType} when parsing DateOnly.");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format)); // Write as a simple string
    }
}
