using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.OpenDouban
{
    internal class StringFloatConverter : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            float.TryParse(reader.GetString(), out var r) ? r : 0f;

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());
    }
}
