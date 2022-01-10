// -----------------------------------------------------------------------
// <copyright file="EnumConverterFactory.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="EnumConverterFactory" />.
    /// </summary>
    internal sealed class EnumConverterFactory : JsonConverterFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumConverterFactory"/> class.
        /// </summary>
        public EnumConverterFactory()
        {
        }

        /// <summary>
        /// The CanConvert.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool CanConvert(Type type)
        {
            return type.IsEnum;
        }

        /// <summary>
        /// The CreateConverter.
        /// </summary>
        /// <param name="type">The type<see cref="Type"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="JsonConverter"/>.</returns>
        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options) =>
            Create(type, EnumConverterOptions.AllowNumbers, options);

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="enumType">The enumType<see cref="Type"/>.</param>
        /// <param name="converterOptions">The converterOptions<see cref="EnumConverterOptions"/>.</param>
        /// <param name="serializerOptions">The serializerOptions<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="JsonConverter"/>.</returns>
        internal static JsonConverter Create(Type enumType, EnumConverterOptions converterOptions, JsonSerializerOptions serializerOptions)
        {
            return (JsonConverter)Activator.CreateInstance(
                GetEnumConverterType(enumType),
                new object[] { converterOptions, serializerOptions })!;
        }

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="enumType">The enumType<see cref="Type"/>.</param>
        /// <param name="converterOptions">The converterOptions<see cref="EnumConverterOptions"/>.</param>
        /// <param name="namingPolicy">The namingPolicy<see cref="JsonNamingPolicy"/>.</param>
        /// <param name="serializerOptions">The serializerOptions<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="JsonConverter"/>.</returns>
        internal static JsonConverter Create(Type enumType, EnumConverterOptions converterOptions, JsonNamingPolicy namingPolicy, JsonSerializerOptions serializerOptions)
        {
            return (JsonConverter)Activator.CreateInstance(
                GetEnumConverterType(enumType),
                new object[] { converterOptions, namingPolicy, serializerOptions })!;
        }

        /// <summary>
        /// The GetEnumConverterType.
        /// </summary>
        /// <param name="enumType">The enumType<see cref="Type"/>.</param>
        /// <returns>The <see cref="Type"/>.</returns>
        private static Type GetEnumConverterType(Type enumType) => typeof(EnumConverter<>).MakeGenericType(enumType);
    }
}
