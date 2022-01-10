// -----------------------------------------------------------------------
// <copyright file="SampleFileSearchQuery.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models
{
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Models.Types;
    using NCloud.FileProviders.AliyunDrive.AliyunDriveAPI.Utils;

    /// <summary>
    /// Defines the <see cref="SampleFileSearchQuery" />.
    /// </summary>
    public class SampleFileSearchQuery
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ParentFileId.
        /// </summary>
        public string ParentFileId { get; set; }

        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        public FileCategoryType? Category { get; set; }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public FileType? Type { get; set; }

        /// <summary>
        /// The GetQueryExpression.
        /// </summary>
        /// <returns>The <see cref="FileSearchQueryExpression"/>.</returns>
        public FileSearchQueryExpression GetQueryExpression()
        {
            var queryExpression = new FileSearchQueryExpression();
            if (Name != null)
                queryExpression.Add(new("name", Name, QueryConditionType.Match));
            if (ParentFileId != null)
                queryExpression.Add(new("parent_file_id", ParentFileId, QueryConditionType.Equal));
            if (Category != null)
                queryExpression.Add(new("category", ReflectionUtils.GetEnumValueName(Category.Value), QueryConditionType.Equal));
            if (Type != null)
                queryExpression.Add(new("type", ReflectionUtils.GetEnumValueName(Type.Value), QueryConditionType.Equal));
            return queryExpression;
        }

        /// <summary>
        /// The ToString.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
            => GetQueryExpression().ToString();
    }
}
