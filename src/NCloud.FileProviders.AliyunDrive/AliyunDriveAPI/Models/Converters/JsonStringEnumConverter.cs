// -----------------------------------------------------------------------
// <copyright file="JsonStringEnumConverter.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="JsonStringEnumConverter" />.
    /// </summary>
    public class JsonStringEnumConverter : JsonConverterFactory
    {
        /// <summary>
        /// Defines the _namingPolicy.
        /// </summary>
        private readonly JsonNamingPolicy _namingPolicy;

        /// <summary>
        /// Defines the _converterOptions.
        /// </summary>
        private readonly EnumConverterOptions _converterOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStringEnumConverter"/> class.
        /// </summary>
        public JsonStringEnumConverter()
            : this(namingPolicy: null, allowIntegerValues: true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStringEnumConverter"/> class.
        /// </summary>
        /// <param name="namingPolicy">The namingPolicy<see cref="JsonNamingPolicy"/>.</param>
        /// <param name="allowIntegerValues">The allowIntegerValues<see cref="bool"/>.</param>
        public JsonStringEnumConverter(JsonNamingPolicy namingPolicy = null, bool allowIntegerValues = true)
        {
            _namingPolicy = namingPolicy;
            _converterOptions = allowIntegerValues
                ? EnumConverterOptions.AllowNumbers | EnumConverterOptions.AllowStrings
                : EnumConverterOptions.AllowStrings;
        }

        /// <inheritdoc />
        public sealed override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        /// <inheritdoc />
        public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            EnumConverterFactory.Create(typeToConvert, _converterOptions, _namingPolicy, options);
    }

    /// <summary>
    /// Defines the EnumConverterOptions.
    /// </summary>
    [Flags]
    internal enum EnumConverterOptions
    {
        /// <summary>
        /// Allow string values.
        /// </summary>
        AllowStrings = 0b0001,
        /// <summary>
        /// Allow number values.
        /// </summary>
        AllowNumbers = 0b0010
    }
}
