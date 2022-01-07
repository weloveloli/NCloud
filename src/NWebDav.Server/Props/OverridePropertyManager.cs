// -----------------------------------------------------------------------
// <copyright file="OverridePropertyManager.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Props
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using NWebDav.Server.Http;
    using NWebDav.Server.Stores;

    /// <summary>
    /// Defines the <see cref="OverridePropertyManager{TEntry}" />.
    /// </summary>
    /// <typeparam name="TEntry">.</typeparam>
    public class OverridePropertyManager<TEntry> : IPropertyManager
        where TEntry : IStoreItem
    {
        /// <summary>
        /// Defines the _converter.
        /// </summary>
        private readonly Func<TEntry, IStoreItem> _converter;

        /// <summary>
        /// Defines the _properties.
        /// </summary>
        private readonly IDictionary<XName, DavProperty<TEntry>> _properties;

        /// <summary>
        /// Defines the _basePropertyManager.
        /// </summary>
        private readonly IPropertyManager _basePropertyManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverridePropertyManager{TEntry}"/> class.
        /// </summary>
        /// <param name="properties">The properties<see cref="IEnumerable{DavProperty{TEntry}}"/>.</param>
        /// <param name="basePropertyManager">The basePropertyManager<see cref="IPropertyManager"/>.</param>
        /// <param name="converter">The converter<see cref="Func{TEntry, IStoreItem} ?"/>.</param>
        public OverridePropertyManager(IEnumerable<DavProperty<TEntry>> properties, IPropertyManager basePropertyManager, Func<TEntry, IStoreItem>? converter = null)
        {
            // Convert the properties to a dictionary for fast retrieval
            _properties = properties?.ToDictionary(p => p.Name) ?? throw new ArgumentNullException(nameof(properties));
            _basePropertyManager = basePropertyManager ?? throw new ArgumentNullException(nameof(basePropertyManager));
            _converter = converter ?? (si => si);

            // Create the property information immediately
            Properties = GetPropertyInfo();
        }

        /// <summary>
        /// Gets the Properties.
        /// </summary>
        public IList<PropertyInfo> Properties { get; }

        /// <summary>
        /// The GetPropertyAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="XName"/>.</param>
        /// <param name="skipExpensive">The skipExpensive<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        public Task<object> GetPropertyAsync(IHttpContext httpContext, IStoreItem item, XName propertyName, bool skipExpensive = false)
        {
            // Find the property
            if (!_properties.TryGetValue(propertyName, out var property))
                return _basePropertyManager.GetPropertyAsync(httpContext, _converter((TEntry)item), propertyName, skipExpensive);

            // Check if the property has a getter
            if (property.GetterAsync == null)
                return _basePropertyManager.GetPropertyAsync(httpContext, _converter((TEntry)item), propertyName, skipExpensive);

            // Skip expensive properties
            if (skipExpensive && property.IsExpensive)
                return Task.FromResult((object)null);

            // Obtain the value
            return property.GetterAsync(httpContext, (TEntry)item);
        }

        /// <summary>
        /// The SetPropertyAsync.
        /// </summary>
        /// <param name="httpContext">The httpContext<see cref="IHttpContext"/>.</param>
        /// <param name="item">The item<see cref="IStoreItem"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="XName"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <returns>The <see cref="Task{DavStatusCode}"/>.</returns>
        public Task<DavStatusCode> SetPropertyAsync(IHttpContext httpContext, IStoreItem item, XName propertyName, object value)
        {
            // Find the property
            if (!_properties.TryGetValue(propertyName, out var property))
                return _basePropertyManager.SetPropertyAsync(httpContext, _converter((TEntry)item), propertyName, value);

            // Check if the property has a setter
            if (property.SetterAsync == null)
                return _basePropertyManager.SetPropertyAsync(httpContext, _converter((TEntry)item), propertyName, value);

            // Set the value
            return property.SetterAsync(httpContext, (TEntry)item, value);
        }

        /// <summary>
        /// The GetPropertyInfo.
        /// </summary>
        /// <returns>The <see cref="IList{PropertyInfo}"/>.</returns>
        private IList<PropertyInfo> GetPropertyInfo()
        {
            // Obtain the base properties that do not have an override
            var basePropertyInfo = _basePropertyManager.Properties.Where(p => !_properties.ContainsKey(p.Name));
            var overridePropertyInfo = _properties.Values.Where(p => p.GetterAsync != null || p.SetterAsync != null).Select(p => new PropertyInfo(p.Name, p.IsExpensive));

            // Combine both lists
            return basePropertyInfo.Concat(overridePropertyInfo).ToList();
        }
    }
}
