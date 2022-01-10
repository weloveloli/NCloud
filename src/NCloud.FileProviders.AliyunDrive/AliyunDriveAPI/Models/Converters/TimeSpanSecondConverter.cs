// -----------------------------------------------------------------------
// <copyright file="TimeSpanSecondConverter.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="TimeSpanSecondConverter" />.
    /// </summary>
    public class TimeSpanSecondConverter : JsonConverter<TimeSpan>
    {
        /// <summary>
        /// The Read.
        /// </summary>
        /// <param name="reader">The reader<see cref="Utf8JsonReader"/>.</param>
        /// <param name="typeToConvert">The typeToConvert<see cref="Type"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="TimeSpan"/>.</returns>
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var text = reader.GetString();
                if (double.TryParse(text, out var sec))
                    return TimeSpan.FromSeconds(sec);
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetDouble(out var sec))
                    return TimeSpan.FromSeconds(sec);
            }
            return default;
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="writer">The writer<see cref="Utf8JsonWriter"/>.</param>
        /// <param name="value">The value<see cref="TimeSpan"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            => writer.WriteNumberValue(value.TotalSeconds);
    }

    /// <summary>
    /// Defines the <see cref="NullableTimeSpanSecondConverter" />.
    /// </summary>
    public class NullableTimeSpanSecondConverter : JsonConverter<TimeSpan?>
    {
        /// <summary>
        /// The Read.
        /// </summary>
        /// <param name="reader">The reader<see cref="Utf8JsonReader"/>.</param>
        /// <param name="typeToConvert">The typeToConvert<see cref="Type"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="TimeSpan?"/>.</returns>
        public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var text = reader.GetString();
                if (double.TryParse(text, out var sec))
                    return TimeSpan.FromSeconds(sec);
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetDouble(out var sec))
                    return TimeSpan.FromSeconds(sec);
            }
            return null;
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="writer">The writer<see cref="Utf8JsonWriter"/>.</param>
        /// <param name="value">The value<see cref="TimeSpan?"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteNumberValue(value.Value.TotalSeconds);
            else
                writer.WriteNullValue();
        }
    }
}
