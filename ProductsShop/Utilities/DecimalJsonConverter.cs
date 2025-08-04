using Newtonsoft.Json;
using System;
using System.Globalization;

namespace ProductsShop.Utilities
{
    public class DecimalJsonConverter : JsonConverter<decimal>
    {
        public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer) =>
        writer.WriteRawValue(value.ToString("F2", CultureInfo.InvariantCulture));

        public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            Convert.ToDecimal(reader.Value);
    }
}
