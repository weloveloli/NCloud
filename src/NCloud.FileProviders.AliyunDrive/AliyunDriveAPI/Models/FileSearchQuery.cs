// -----------------------------------------------------------------------
// <copyright file="FileSearchQuery.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Utils;

    /// <summary>
    /// Defines the <see cref="FileSearchQuery" />.
    /// </summary>
    public class FileSearchQuery
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the ConditionType.
        /// </summary>
        public QueryConditionType ConditionType { get; set; }

        /// <summary>
        /// Gets or sets the NearRelationType.
        /// </summary>
        public QueryRelationType NearRelationType { get; set; } = QueryRelationType.AND;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchQuery"/> class.
        /// </summary>
        public FileSearchQuery()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchQuery"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="type">The type<see cref="QueryConditionType"/>.</param>
        public FileSearchQuery(string name, object value, QueryConditionType type)
        {
            Name = name;
            Value = value;
            ConditionType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearchQuery"/> class.
        /// </summary>
        /// <param name="name">The name<see cref="string"/>.</param>
        /// <param name="value">The value<see cref="object"/>.</param>
        /// <param name="type">The type<see cref="QueryConditionType"/>.</param>
        /// <param name="relationType">The relationType<see cref="QueryRelationType"/>.</param>
        public FileSearchQuery(string name, object value, QueryConditionType type, QueryRelationType relationType)
        {
            Name = name;
            Value = value;
            ConditionType = type;
            NearRelationType = relationType;
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            var isString = Value is string;
            return isString ? $"{Name} {ReflectionUtils.GetEnumValueName(ConditionType)} \"{Value}\""
                : $"{Name} {ReflectionUtils.GetEnumValueName(ConditionType)} {Value}";
        }
    }
}
