// -----------------------------------------------------------------------
// <copyright file="ReflectionUtils.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Utils
{
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Defines the <see cref="ReflectionUtils" />.
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// The GetEnumValueName.
        /// </summary>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetEnumValueName(object value)
        {
            var enumType = value.GetType();
            var member = enumType.GetMember(value.ToString()).FirstOrDefault(m => m.DeclaringType == enumType);
            if (member != null)
            {
                var attr = member.GetCustomAttribute(typeof(JsonPropertyNameAttribute), false);
                if (attr != null)
                    return (attr as JsonPropertyNameAttribute).Name;
            }
            return value.ToString();
        }
    }
}
