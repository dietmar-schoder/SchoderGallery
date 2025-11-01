using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchoderGallery.Helpers;

public static class Extensions
{
    private static readonly JsonSerializerOptions DefaultOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    public static T FromJson<T>(this string json) =>
        json is null ? default : JsonSerializer.Deserialize<T>(json, DefaultOptions);

    public static string ToJson<T>(this T obj) =>
        JsonSerializer.Serialize(obj, DefaultOptions);
}
