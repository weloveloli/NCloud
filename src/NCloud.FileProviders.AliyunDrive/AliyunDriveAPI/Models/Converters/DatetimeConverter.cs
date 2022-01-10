// -----------------------------------------------------------------------
// <copyright file="DatetimeConverter.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="DatetimeConverter" />.
    /// </summary>
    public class DatetimeConverter : JsonConverter<DateTime?>
    {
        /// <summary>
        /// The Read.
        /// </summary>
        /// <param name="reader">The reader<see cref="Utf8JsonReader"/>.</param>
        /// <param name="typeToConvert">The typeToConvert<see cref="Type"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="DateTime?"/>.</returns>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (string.IsNullOrWhiteSpace(reader.GetString()))
                {
                    return null;
                }
                if (DateTime.TryParse(reader.GetString(), out var date))
                    return date;
            }
            return reader.GetDateTime();
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="writer">The writer<see cref="Utf8JsonWriter"/>.</param>
        /// <param name="value">The value<see cref="DateTime?"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                // 2021-12-15T16:04:05.148Z
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            }
            else
            {
                writer.WriteStringValue(string.Empty);
            }
        }
    }
}
