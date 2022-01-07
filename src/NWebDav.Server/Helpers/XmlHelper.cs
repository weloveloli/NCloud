// -----------------------------------------------------------------------
// <copyright file="XmlHelper.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NWebDav.Server.Helpers
{
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the <see cref="XmlHelper" />.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// The GetXmlValue.
        /// </summary>
        /// <typeparam name="TEnum">.</typeparam>
        /// <param name="value">The value<see cref="TEnum"/>.</param>
        /// <param name="defaultValue">The defaultValue<see cref="string?"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetXmlValue<TEnum>(TEnum value, string? defaultValue = null) where TEnum : struct
        {
            // Obtain the member information
            var memberInfo = typeof(TEnum).GetMember(value.ToString()).FirstOrDefault();
            if (memberInfo == null)
                return defaultValue;

            var xmlEnumAttribute = memberInfo.GetCustomAttribute<XmlEnumAttribute>();
            return xmlEnumAttribute?.Name;
        }
    }
}
