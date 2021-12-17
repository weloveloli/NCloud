// -----------------------------------------------------------------------
// <copyright file="ProviderConfigConverter.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using NCloud.FileProviders.Abstractions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Defines the <see cref="ProviderConfigConverter" />.
    /// </summary>
    public class ProviderConfigConverter : JsonConverter
    {
        /// <summary>
        /// Defines the _providerConfigTypes.
        /// </summary>
        private IDictionary<string, Type> _providerConfigTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderConfigConverter"/> class.
        /// </summary>
        public ProviderConfigConverter()
        {
            var fileProviderAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string dll in Directory.GetFiles(path, "NCloud.FileProviders.*.dll"))
            {
                fileProviderAssemblies.Add(Assembly.LoadFrom(dll));
            }
            var types = fileProviderAssemblies.SelectMany(e => e.GetExportedTypes())
                .Where(e => e.IsAssignableTo(typeof(INCloudFileProvider)))
                .Where(e => e.GetCustomAttributes(typeof(FileProviderAttribute), false).Length == 1);
            this._providerConfigTypes = types
                .Select(e => (((FileProviderAttribute)e.GetCustomAttributes(typeof(FileProviderAttribute), false)[0]).Type, e))
                .ToDictionary(e => e.Type, e => GetGenericConfigType(e.e));
        }

        /// <summary>
        /// The CanConvert.
        /// </summary>
        /// <param name="objectType">The objectType<see cref="Type"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(BaseProviderConfig).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// The ReadJson.
        /// </summary>
        /// <param name="reader">The reader<see cref="JsonReader"/>.</param>
        /// <param name="objectType">The objectType<see cref="Type"/>.</param>
        /// <param name="existingValue">The existingValue<see cref="object"/>.</param>
        /// <param name="serializer">The serializer<see cref="JsonSerializer"/>.</param>
        /// <returns>The <see cref="object"/>.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            var jsonObject = JObject.Load(reader);
            object target = null;
            JToken type;
            if (jsonObject.TryGetValue("Type", out type))
            {
                if (!this._providerConfigTypes.ContainsKey(type.ToString()))
                {
                    return null;
                }
                var configType = this._providerConfigTypes[type.ToString()];
                target = Activator.CreateInstance(configType);
            }
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        /// <summary>
        /// The WriteJson.
        /// </summary>
        /// <param name="writer">The writer<see cref="JsonWriter"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="serializer">The serializer<see cref="JsonSerializer"/>.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The GetGenericConfigType.
        /// </summary>
        /// <returns>The <see cref="Type"/>.</returns>
        public Type GetGenericConfigType(Type type)
        {
            while (!type.IsGenericType)
            {
                type = type.BaseType;
            }
            return type.GetGenericArguments()[0];
        }
    }
}
