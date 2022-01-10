// -----------------------------------------------------------------------
// <copyright file="EnumConverter.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Converters
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="EnumConverter{T}" />.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    internal sealed class EnumConverter<T> : JsonConverter<T>
            where T : struct, Enum
    {
        /// <summary>
        /// Defines the s_enumTypeCode.
        /// </summary>
        private static readonly TypeCode s_enumTypeCode = Type.GetTypeCode(typeof(T));

        /// <summary>
        /// Defines the s_negativeSign.
        /// </summary>
        private static readonly string s_negativeSign = (int)s_enumTypeCode % 2 == 0 ? null : NumberFormatInfo.CurrentInfo.NegativeSign;

        /// <summary>
        /// Defines the ValueSeparator.
        /// </summary>
        private const string ValueSeparator = ", ";

        /// <summary>
        /// Defines the _converterOptions.
        /// </summary>
        private readonly EnumConverterOptions _converterOptions;

        /// <summary>
        /// Defines the _namingPolicy.
        /// </summary>
        private readonly JsonNamingPolicy _namingPolicy;

        /// <summary>
        /// Defines the _nameCache.
        /// </summary>
        private readonly ConcurrentDictionary<ulong, JsonEncodedText> _nameCache;

        /// <summary>
        /// Defines the _dictionaryKeyPolicyCache.
        /// </summary>
        private ConcurrentDictionary<ulong, JsonEncodedText> _dictionaryKeyPolicyCache;

        /// <summary>
        /// Defines the NameCacheSizeSoftLimit.
        /// </summary>
        private const int NameCacheSizeSoftLimit = 64;

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
        /// Initializes a new instance of the <see cref="EnumConverter{T}"/> class.
        /// </summary>
        /// <param name="converterOptions">The converterOptions<see cref="EnumConverterOptions"/>.</param>
        /// <param name="serializerOptions">The serializerOptions<see cref="JsonSerializerOptions"/>.</param>
        public EnumConverter(EnumConverterOptions converterOptions, JsonSerializerOptions serializerOptions)
            : this(converterOptions, namingPolicy: null, serializerOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumConverter{T}"/> class.
        /// </summary>
        /// <param name="converterOptions">The converterOptions<see cref="EnumConverterOptions"/>.</param>
        /// <param name="namingPolicy">The namingPolicy<see cref="JsonNamingPolicy"/>.</param>
        /// <param name="serializerOptions">The serializerOptions<see cref="JsonSerializerOptions"/>.</param>
        public EnumConverter(EnumConverterOptions converterOptions, JsonNamingPolicy namingPolicy, JsonSerializerOptions serializerOptions)
        {
            _converterOptions = converterOptions;
            _namingPolicy = namingPolicy;
            _nameCache = new ConcurrentDictionary<ulong, JsonEncodedText>();

            var names = Enum.GetNames(typeof(T));
            var values = Enum.GetValues(typeof(T));
            Debug.Assert(names.Length == values.Length);

            var encoder = serializerOptions.Encoder;

            for (var i = 0; i < names.Length; i++)
            {
                if (_nameCache.Count >= NameCacheSizeSoftLimit)
                {
                    break;
                }

                var value = (T)values.GetValue(i)!;
                var key = ConvertToUInt64(value);
                var name = names[i];

                _nameCache.TryAdd(
                    key,
                    namingPolicy == null
                        ? JsonEncodedText.Encode(name, encoder)
                        : FormatEnumValue(name, encoder));
            }
        }

        /// <summary>
        /// The Read.
        /// </summary>
        /// <param name="reader">The reader<see cref="Utf8JsonReader"/>.</param>
        /// <param name="typeToConvert">The typeToConvert<see cref="Type"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var token = reader.TokenType;

            if (token == JsonTokenType.String)
            {
                var (isOk, value) = GetEnumValueByName(reader.GetString());
                if (isOk)
                    return value;
                if (!_converterOptions.HasFlag(EnumConverterOptions.AllowStrings))
                {
                    ThrowJsonException();
                    return default;
                }

                return ReadAsPropertyNameCore(ref reader, typeToConvert, options);
            }

            if (token != JsonTokenType.Number || !_converterOptions.HasFlag(EnumConverterOptions.AllowNumbers))
            {
                ThrowJsonException();
                return default;
            }

            switch (s_enumTypeCode)
            {
                // Switch cases ordered by expected frequency

                case TypeCode.Int32:
                    if (reader.TryGetInt32(out var int32))
                    {
                        return Unsafe.As<int, T>(ref int32);
                    }
                    break;
                case TypeCode.UInt32:
                    if (reader.TryGetUInt32(out var uint32))
                    {
                        return Unsafe.As<uint, T>(ref uint32);
                    }
                    break;
                case TypeCode.UInt64:
                    if (reader.TryGetUInt64(out var uint64))
                    {
                        return Unsafe.As<ulong, T>(ref uint64);
                    }
                    break;
                case TypeCode.Int64:
                    if (reader.TryGetInt64(out var int64))
                    {
                        return Unsafe.As<long, T>(ref int64);
                    }
                    break;
                case TypeCode.SByte:
                    if (reader.TryGetSByte(out var byte8))
                    {
                        return Unsafe.As<sbyte, T>(ref byte8);
                    }
                    break;
                case TypeCode.Byte:
                    if (reader.TryGetByte(out var ubyte8))
                    {
                        return Unsafe.As<byte, T>(ref ubyte8);
                    }
                    break;
                case TypeCode.Int16:
                    if (reader.TryGetInt16(out var int16))
                    {
                        return Unsafe.As<short, T>(ref int16);
                    }
                    break;
                case TypeCode.UInt16:
                    if (reader.TryGetUInt16(out var uint16))
                    {
                        return Unsafe.As<ushort, T>(ref uint16);
                    }
                    break;
            }

            ThrowJsonException();
            return default;
        }

        /// <summary>
        /// The GetEnumValueByName.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <returns>The <see cref="(bool IsOk, T Value)"/>.</returns>
        private (bool IsOk, T Value) GetEnumValueByName(string name)
        {
            var enumType = typeof(T);
            var members = enumType.GetMembers().Where(m => m.DeclaringType == enumType);
            foreach (var member in members)
            {
                var memberName = member.Name;
                var attr = member.GetCustomAttribute(typeof(JsonPropertyNameAttribute), false);
                if (attr != null)
                {
                    memberName = (attr as JsonPropertyNameAttribute).Name;
                }
                if (memberName == name && Enum.TryParse<T>(member.Name, out var value))
                    return (true, value);
            }
            return (false, default);
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="writer">The writer<see cref="Utf8JsonWriter"/>.</param>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // If strings are allowed, attempt to write it out as a string value
            if (_converterOptions.HasFlag(EnumConverterOptions.AllowStrings))
            {
                var key = ConvertToUInt64(value);

                var enumType = typeof(T);
                var member = enumType.GetMember(value.ToString()).FirstOrDefault(m => m.DeclaringType == enumType);
                if (member != null)
                {
                    var attr = member.GetCustomAttribute(typeof(JsonPropertyNameAttribute), false);
                    if (attr != null)
                    {
                        writer.WriteStringValue((attr as JsonPropertyNameAttribute).Name);
                        return;
                    }
                }

                if (_nameCache.TryGetValue(key, out var formatted))
                {
                    writer.WriteStringValue(formatted);
                    return;
                }

                var original = value.ToString();
                if (IsValidIdentifier(original))
                {
                    // We are dealing with a combination of flag constants since
                    // all constant values were cached during warm-up.
                    var encoder = options.Encoder;

                    if (_nameCache.Count < NameCacheSizeSoftLimit)
                    {
                        formatted = _namingPolicy == null
                            ? JsonEncodedText.Encode(original, encoder)
                            : FormatEnumValue(original, encoder);

                        writer.WriteStringValue(formatted);

                        _nameCache.TryAdd(key, formatted);
                    }
                    else
                    {
                        // We also do not create a JsonEncodedText instance here because passing the string
                        // directly to the writer is cheaper than creating one and not caching it for reuse.
                        writer.WriteStringValue(
                            _namingPolicy == null
                            ? original
                            : FormatEnumValueToString(original, encoder));
                    }

                    return;
                }
            }

            if (!_converterOptions.HasFlag(EnumConverterOptions.AllowNumbers))
            {
                ThrowJsonException();
            }

            switch (s_enumTypeCode)
            {
                case TypeCode.Int32:
                    writer.WriteNumberValue(Unsafe.As<T, int>(ref value));
                    break;
                case TypeCode.UInt32:
                    writer.WriteNumberValue(Unsafe.As<T, uint>(ref value));
                    break;
                case TypeCode.UInt64:
                    writer.WriteNumberValue(Unsafe.As<T, ulong>(ref value));
                    break;
                case TypeCode.Int64:
                    writer.WriteNumberValue(Unsafe.As<T, long>(ref value));
                    break;
                case TypeCode.Int16:
                    writer.WriteNumberValue(Unsafe.As<T, short>(ref value));
                    break;
                case TypeCode.UInt16:
                    writer.WriteNumberValue(Unsafe.As<T, ushort>(ref value));
                    break;
                case TypeCode.Byte:
                    writer.WriteNumberValue(Unsafe.As<T, byte>(ref value));
                    break;
                case TypeCode.SByte:
                    writer.WriteNumberValue(Unsafe.As<T, sbyte>(ref value));
                    break;
                default:
                    ThrowJsonException();
                    break;
            }
        }

        /// <summary>
        /// The ConvertToUInt64.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <returns>The <see cref="ulong"/>.</returns>
        private static ulong ConvertToUInt64(object value)
        {
            Debug.Assert(value is T);
            var result = s_enumTypeCode switch
            {
                TypeCode.Int32 => (ulong)(int)value,
                TypeCode.UInt32 => (uint)value,
                TypeCode.UInt64 => (ulong)value,
                TypeCode.Int64 => (ulong)(long)value,
                TypeCode.SByte => (ulong)(sbyte)value,
                TypeCode.Byte => (byte)value,
                TypeCode.Int16 => (ulong)(short)value,
                TypeCode.UInt16 => (ushort)value,
                _ => throw new InvalidOperationException(),
            };
            return result;
        }

        /// <summary>
        /// The IsValidIdentifier.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool IsValidIdentifier(string value)
        {
            // Trying to do this check efficiently. When an enum is converted to
            // string the underlying value is given if it can't find a matching
            // identifier (or identifiers in the case of flags).
            //
            // The underlying value will be given back with a digit (e.g. 0-9) possibly
            // preceded by a negative sign. Identifiers have to start with a letter
            // so we'll just pick the first valid one and check for a negative sign
            // if needed.
            return value[0] >= 'A' &&
                (s_negativeSign == null || !value.StartsWith(s_negativeSign));
        }

        /// <summary>
        /// The FormatEnumValue.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="encoder">The encoder<see cref="JavaScriptEncoder"/>.</param>
        /// <returns>The <see cref="JsonEncodedText"/>.</returns>
        private JsonEncodedText FormatEnumValue(string value, JavaScriptEncoder encoder)
        {
            Debug.Assert(_namingPolicy != null);
            var formatted = FormatEnumValueToString(value, encoder);
            return JsonEncodedText.Encode(formatted, encoder);
        }

        /// <summary>
        /// The FormatEnumValueToString.
        /// </summary>
        /// <param name="value">The value<see cref="string"/>.</param>
        /// <param name="encoder">The encoder<see cref="JavaScriptEncoder"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string FormatEnumValueToString(string value, JavaScriptEncoder encoder)
        {
            Debug.Assert(_namingPolicy != null);

            string converted;
            if (!value.Contains(ValueSeparator))
            {
                converted = _namingPolicy.ConvertName(value);
            }
            else
            {
                // todo: optimize implementation here by leveraging https://github.com/dotnet/runtime/issues/934.
                var enumValues = value.Split(
#if BUILDING_INBOX_LIBRARY
                    ValueSeparator
#else
                    new string[] { ValueSeparator }, StringSplitOptions.None
#endif
                    );

                for (var i = 0; i < enumValues.Length; i++)
                {
                    enumValues[i] = _namingPolicy.ConvertName(enumValues[i]);
                }

                converted = string.Join(ValueSeparator, enumValues);
            }

            return converted;
        }

        /// <summary>
        /// The ReadAsPropertyNameCore.
        /// </summary>
        /// <param name="reader">The reader<see cref="Utf8JsonReader"/>.</param>
        /// <param name="typeToConvert">The typeToConvert<see cref="Type"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        /// <returns>The <see cref="T"/>.</returns>
        internal T ReadAsPropertyNameCore(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();

            // Try parsing case sensitive first
            if (!Enum.TryParse(enumString, out T value)
                && !Enum.TryParse(enumString, ignoreCase: true, out value))
            {
                ThrowJsonException();
            }

            return value;
        }

        /// <summary>
        /// The WriteAsPropertyNameCore.
        /// </summary>
        /// <param name="writer">The writer<see cref="Utf8JsonWriter"/>.</param>
        /// <param name="value">The value<see cref="T"/>.</param>
        /// <param name="options">The options<see cref="JsonSerializerOptions"/>.</param>
        /// <param name="isWritingExtensionDataProperty">The isWritingExtensionDataProperty<see cref="bool"/>.</param>
        internal void WriteAsPropertyNameCore(Utf8JsonWriter writer, T value, JsonSerializerOptions options, bool isWritingExtensionDataProperty)
        {
            // An EnumConverter that invokes this method
            // can only be created by JsonSerializerOptions.GetDictionaryKeyConverter
            // hence no naming policy is expected.
            Debug.Assert(_namingPolicy == null);

            var key = ConvertToUInt64(value);

            // Try to obtain values from caches
            if (options.DictionaryKeyPolicy != null)
            {
                Debug.Assert(!isWritingExtensionDataProperty);

                if (_dictionaryKeyPolicyCache != null && _dictionaryKeyPolicyCache.TryGetValue(key, out var formatted))
                {
                    writer.WritePropertyName(formatted);
                    return;
                }
            }
            else if (_nameCache.TryGetValue(key, out var formatted))
            {
                writer.WritePropertyName(formatted);
                return;
            }

            // if there are not cached values
            var original = value.ToString();
            if (IsValidIdentifier(original))
            {
                if (options.DictionaryKeyPolicy != null)
                {
                    original = options.DictionaryKeyPolicy.ConvertName(original);

                    if (original == null)
                        throw new InvalidOperationException();

                    _dictionaryKeyPolicyCache ??= new ConcurrentDictionary<ulong, JsonEncodedText>();

                    if (_dictionaryKeyPolicyCache.Count < NameCacheSizeSoftLimit)
                    {
                        var encoder = options.Encoder;

                        var formatted = JsonEncodedText.Encode(original, encoder);

                        writer.WritePropertyName(formatted);

                        _dictionaryKeyPolicyCache.TryAdd(key, formatted);
                    }
                    else
                    {
                        // We also do not create a JsonEncodedText instance here because passing the string
                        // directly to the writer is cheaper than creating one and not caching it for reuse.
                        writer.WritePropertyName(original);
                    }

                    return;
                }
                else
                {
                    // We might be dealing with a combination of flag constants since all constant values were
                    // likely cached during warm - up(assuming the number of constants <= NameCacheSizeSoftLimit).

                    var encoder = options.Encoder;

                    if (_nameCache.Count < NameCacheSizeSoftLimit)
                    {
                        var formatted = JsonEncodedText.Encode(original, encoder);

                        writer.WritePropertyName(formatted);

                        _nameCache.TryAdd(key, formatted);
                    }
                    else
                    {
                        // We also do not create a JsonEncodedText instance here because passing the string
                        // directly to the writer is cheaper than creating one and not caching it for reuse.
                        writer.WritePropertyName(original);
                    }

                    return;
                }
            }

            switch (s_enumTypeCode)
            {
                case TypeCode.Int32:
                    writer.WriteNumberValue(Unsafe.As<T, int>(ref value));

                    break;
                case TypeCode.UInt32:
                    writer.WriteNumberValue(Unsafe.As<T, uint>(ref value));
                    break;
                case TypeCode.UInt64:
                    writer.WriteNumberValue(Unsafe.As<T, ulong>(ref value));
                    break;
                case TypeCode.Int64:
                    writer.WriteNumberValue(Unsafe.As<T, long>(ref value));
                    break;
                case TypeCode.Int16:
                    writer.WriteNumberValue(Unsafe.As<T, short>(ref value));
                    break;
                case TypeCode.UInt16:
                    writer.WriteNumberValue(Unsafe.As<T, ushort>(ref value));
                    break;
                case TypeCode.Byte:
                    writer.WriteNumberValue(Unsafe.As<T, byte>(ref value));
                    break;
                case TypeCode.SByte:
                    writer.WriteNumberValue(Unsafe.As<T, sbyte>(ref value));
                    break;
                default:
                    ThrowJsonException();
                    break;
            }
        }

        /// <summary>
        /// The ThrowJsonException.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        private static void ThrowJsonException(string message = null)
            => throw new JsonException(message);
    }
}
