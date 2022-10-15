using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Transcore.TagInventory.Api.JsonConverter
{
    public class ApplicationDateTimeConverter : JsonConverter<DateTime?>
    {
        private const string Format = "MM-dd-yyyy";
        private const string FormatWithTime = @"yyyy-MM-dd\THH:mm:ss";
        private const string FormatWithTimeSecondsPercesion = @"yyyy-MM-dd\THH:mm:ss.000Z";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            var value = reader.GetString();

            //DateTime? parsedDateTime = null;

            DateTime outDate;

            if (!DateTime.TryParseExact(value, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out outDate))
            {
                if (!DateTime.TryParseExact(value, FormatWithTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out outDate))
                {

                    if (DateTime.TryParseExact(value, FormatWithTimeSecondsPercesion, CultureInfo.InvariantCulture, DateTimeStyles.None, out outDate))
                    {
                        return outDate;
                    }
                    return outDate;
                }

            }

            return outDate;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value is { })
            {
                writer.WriteStringValue(value.Value.ToString(Format, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
