// -----------------------------------------------------------------------
// <copyright file="JsonNodeConverter.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="JsonNodeConverter" />.
    /// </summary>
    public class JsonNodeConverter : JsonConverter<JsonNode>
    {
        /// <summary>
        /// The Read.
        /// </summary>
        /// <param name="reader">The reader<see cref="Utf8JsonReader"/>.</param>
        /// <param name="typeToConvert">The typeToConvert<see cref="Type"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="JsonNode"/>.</returns>
        public override JsonNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = reader.GetString();
            return json != null ? JsonNode.Parse(json) : null;
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="writer">The writer<see cref="Utf8JsonWriter"/>.</param>
        /// <param name="value">The value<see cref="JsonNode"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        public override void Write(Utf8JsonWriter writer, JsonNode value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value.ToString());
        }
    }
}
